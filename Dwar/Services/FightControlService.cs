using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class FightControlService : IHandleFightState, IComboSetService
    {
        public const string Victory = "fightover_victory.ogg";
        public const string Defeat = "fightover_defeat.ogg";
        public const string StartFight = "combo.css";

        private const string WinnTemplate = "win.png";
        private IBitmapRepository _bitmapRepository;
        private StartFightService _startFightService;
        private IUserInput _userInput;
        private Combo _combo = null!;
        private FightState _state;

        public FightControlService(IBitmapRepository bitmapRepository,
                                   StartFightService startFightService,
                                   IUserInput userInput)
        {
            _bitmapRepository = bitmapRepository;
            _startFightService = startFightService;
            _userInput = userInput;
        }

        public void SetCombo(Combo combo)
        {
            _combo = combo ?? throw new ArgumentNullException(nameof(combo));
        }

        public async Task Fight()
        {
            if (_combo.FightInDefence)
            {
                await _startFightService.WaitCannAttackAsync();
                _userInput.Left();
            }

            while (FightFinish() == false)
            {
                await _startFightService.WaitCannAttackAsync();

                var nextStep = _combo.GetNext();
                await Task.Delay(nextStep.Delay * 1000);

                switch (nextStep.Type)
                {
                    case ComboStepType.Up:
                        _userInput.Up();
                        break;
                    case ComboStepType.Down:
                        _userInput.Down();
                        break;
                    case ComboStepType.Forward:
                        _userInput.Right();
                        break;
                }
            }
        }
        private bool FightFinish()
        {
            return _state == FightState.Stop;
        }

        public void HandleRequest(string url)
        {
            if (url.Contains(Victory))
                _state = FightState.Stop;
            if (url.Contains(Defeat))
                _state = FightState.Stop;
            if (url.Contains(StartFight))
                _state = FightState.Running;
        }
        public enum FightState
        {
            Running = 0,
            Stop = 1
        }
    }
}
