using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
        private ILog _log;
        private INotifyer _notifyer;
        public FightService(HttpService actionHttpService,
                                  RefreshService mouseService,
                                  StartFightService startFightService,
                                  FightControlService fightControlService,
                                  IActionRepository actionRepository,
                                  ILog log,
                                  INotifyer notifyer)
        {
            _actionHttpService = actionHttpService;
            _mouseService = mouseService;
            _startFightService = startFightService;
            _fightControlService = fightControlService;
            _actionRepository = actionRepository;
            _log = log;
            _notifyer = notifyer;
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
            _startFightService.StopFight();
            _notifyer.Notify("Attack");
            while (await _actionHttpService.ExecuteAsync(_actionRepository.Get(_fightConfig.AttackId)) == false)
            {
                await Task.Delay(100);
                if (stopBot.Stop)
                {
                    return;
                }
            }
            _startFightService.OnStartFight();

            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
            
            _notifyer.Notify("Call extra actions");

            foreach (var action in actions)
            {
                while( await _actionHttpService.ExecuteAsync(action) == false)
                {
                    await Task.Delay(100);
                    if (stopBot.Stop)
                        return;
                }
            }

            await _startFightService.WaitCannAttackAsync(stopBot); //wait start bot => screen analyse

            await _fightControlService.Fight(stopBot); //fight => screen analyse
        }
    }
}
