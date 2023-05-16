using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class ActionFightService : IActionService, IActionSetService
    {
        private HttpService _actionHttpService;
        private RefreshService _mouseService;
        private StartFightService _startFightService;
        private FightControlService _fightControlService;
        private Fight? _fightConfig;

        public ActionFightService(HttpService actionHttpService,
                                  RefreshService mouseService,
                                  StartFightService startFightService,
                                  FightControlService fightControlService)
        {
            _actionHttpService = actionHttpService;
            _mouseService = mouseService;
            _startFightService = startFightService;
            _fightControlService = fightControlService;
        }
        public void SetAttack(Fight fightConfig)
        {
            _fightConfig = fightConfig;
        }

        public async Task ExecuteAsync()
        {
            if (_fightConfig == null)
            {
                throw new InvalidOperationException("Attack not setted");
            }

            await _actionHttpService.ExecuteAsync(_fightConfig.Attack); //Attack => http
            await _mouseService.ClickHunt(); //Click => ohota => mouse

            await _startFightService.WaitCannAttackAsync(); //wait start bot => screen analyse


            //Call somthing => http
            foreach (var action in _fightConfig.StartUpActions)
            {
                await _actionHttpService.ExecuteAsync(action); 
            }

            await _fightControlService.Fight(); //fight => screen analyse
        }
    }
}
