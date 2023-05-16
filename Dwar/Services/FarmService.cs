using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class FarmService : IActionService,IActionSetService
    {
        private HttpService _actionHttpService;
        private RefreshService _mouseService;
        private IActionRepository _actionRepository;
        private FarmEndService _farmEndService;
        private Fight? _fightConfig;

        public FarmService(RefreshService mouseService, IActionRepository actionRepository, HttpService actionHttpService, FarmEndService farmEndService)
        {
            _mouseService = mouseService;
            _actionRepository = actionRepository;
            _actionHttpService = actionHttpService;
            _farmEndService = farmEndService;
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
            
            bool result;
            var actionAttack = _actionRepository.Get(_fightConfig.AttackId);
            
            result = await _actionHttpService.ExecuteAsync(actionAttack);

            //await Task.Delay(1000);
            await _mouseService.ClickHunt();
            
            _farmEndService.SetStartFarm();
            await _farmEndService.WaitEnd(120, stopBot);

            await Task.Delay(1000);
            
            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
            foreach (var action in actions)
            {
                await _actionHttpService.ExecuteAsync(action);
            }

        }
    }
}
