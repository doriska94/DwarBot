using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class ActionFightService : IActionService
    {
        private HttpService _actionHttpService;
        private MouseService _mouseService;
        private StartFightService _startFightService;
        private FightControlService _fightControlService;
        private Action? _attack;

        public ActionFightService(HttpService actionHttpService,
                                  MouseService mouseService,
                                  StartFightService startFightService,
                                  FightControlService fightControlService)
        {
            _actionHttpService = actionHttpService;
            _mouseService = mouseService;
            _startFightService = startFightService;
            _fightControlService = fightControlService;
        }
        public void SetAttack(Action attack)
        {
            _attack = attack;
        }

        public async Task ExecuteAsync()
        {
            if(_attack == null)
            {
                throw new InvalidOperationException("Attack not setted");
            }

            await _actionHttpService.ExecuteAsync(_attack); //Attack => http
            await _mouseService.ClickHunt(); //Click => ohota => mouse

            await _startFightService.WaitCannAttackAsync(); //wait start bot => screen analyse
            
            _startFightService.SetFightFocus();

            //Todo Call somthing => http

            await _fightControlService.Fight(); //fight => screen analyse
        }
    }
}
