using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class FightService : IActionService, IActionSetService
    {
        private HttpService _actionHttpService;
        private RefreshService _mouseService;
        private StartFightService _startFightService;
        private FightControlService _fightControlService;
        private Fight? _fightConfig;
        private IActionRepository _actionRepository;

        public FightService(HttpService actionHttpService,
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

        public async Task ExecuteAsync(StopBotCommand stopBot)
        {
            if (_fightConfig == null)
            {
                throw new InvalidOperationException("Attack not setted");
            }

            await _actionHttpService.ExecuteAsync(_actionRepository.Get(_fightConfig.AttackId)); //Attack => http
            while (_startFightService.IsFightStarted() == false)
            {
                await Task.Delay(100);

            }
            await Task.Delay(1000);

            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);

            foreach (var action in actions)
            {
                await _actionHttpService.ExecuteAsync(action); 
            }

            await _startFightService.WaitCannAttackAsync(stopBot); //wait start bot => screen analyse

            await _fightControlService.Fight(stopBot); //fight => screen analyse
        }
    }
}
