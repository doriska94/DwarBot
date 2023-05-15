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
    public class FightControlService
    {
        private const string WinnTemplate = "win.png";
        private IBitmapRepository _bitmapRepository;
        private IScreenshot _screenshot;
        private StartFightService _startFightService;
        private IUserInput _userInput;
        private Combo _combo = null!;

        public FightControlService(IBitmapRepository bitmapRepository,
                                   IScreenshot screenshot,
                                   StartFightService startFightService,
                                   IUserInput userInput)
        {
            _bitmapRepository = bitmapRepository;
            _screenshot = screenshot;
            _startFightService = startFightService;
            _userInput = userInput;
        }

        public void SetCombo(Combo combo)
        {
            _combo = combo?? throw new ArgumentNullException(nameof(combo));
        }

        public async Task Fight()
        {
            if(_combo.FightInDefence)
            {
                await _startFightService.WaitCannAttackAsync();
                _startFightService.SetFightFocus();
                _userInput.Left();
            }

            while (FightFinish())
            {
                await _startFightService.WaitCannAttackAsync();
                _startFightService.SetFightFocus();

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
            var winnPoint = GetTemplatePosition(WinnTemplate);
            return winnPoint != Point.Empty;
        }

        private Point GetTemplatePosition(string template)
        {
            Bitmap screen = _screenshot.TakeScreenShot();
            var searchTemplate = _bitmapRepository.Get(template);
            var point = screen.FindPosition(searchTemplate);
            return point;
        }
    }
}
