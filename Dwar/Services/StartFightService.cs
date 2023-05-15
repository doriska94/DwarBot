using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Dwar.Services.FightControlService;

namespace Dwar.Services
{
    public class StartFightService : IHandleFightState
    {
        private const string Template = "can_atack.png";
        private const int WaitStartFight = 500;
        private FightState _state;
        private IBitmapRepository _bitmapRepository;

        public StartFightService(IBitmapRepository bitmapRepository)
        {
            _bitmapRepository = bitmapRepository;
        }

        public async Task WaitCannAttackAsync()
        {
            while (IsFightStarted() == false)
            {
                await Task.Delay(WaitStartFight);
            }
        }


        public bool IsFightStarted()
        {
            return _state == FightState.Running;
        }
        public void HandleRequest(string url)
        {
            if (url.Contains(FightControlService.Victory))
                _state = FightState.Stop;
            if (url.Contains(FightControlService.Defeat))
                _state = FightState.Stop;
            if (url.Contains(FightControlService.StartFight))
                _state = FightState.Running;
        }

    }
}
