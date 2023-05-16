using Dwar.Repositorys;
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
        private IActionRepository _actionRepository;

        public ActionFightService(HttpService actionHttpService,
                                  RefreshService mouseService,
                                  StartFightService startFightService,
                                  FightControlService fightControlService,
                                  IActionRepository actionRepository)
        {
            _actionHttpService = actionHttpService;
            _mouseService = mouseService;
            _startFightService = startFightService;
            _fightControlService = fightControlService;
            _actionRepository = actionRepository;
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

            await _actionHttpService.ExecuteAsync(_actionRepository.Get(_fightConfig.AttackId)); //Attack => http
            await _mouseService.ClickHunt(); //Click => ohota => mouse

            await _startFightService.WaitCannAttackAsync(); //wait start bot => screen analyse

            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
            //Call somthing => http
            foreach (var action in actions)
            {
                await _actionHttpService.ExecuteAsync(action); 
            }

            await _fightControlService.Fight(); //fight => screen analyse
        }
    }
}
