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
        private Fight? _fightConfig;

        public FarmService(RefreshService mouseService, IActionRepository actionRepository, HttpService actionHttpService)
        {
            _mouseService = mouseService;
            _actionRepository = actionRepository;
            _actionHttpService = actionHttpService;
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
            
            await _mouseService.ClickHunt();

            await _actionHttpService.ExecuteAsync(_actionRepository.Get(_fightConfig.AttackId));

            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
            foreach (var action in actions)
            {
                await _actionHttpService.ExecuteAsync(action);
            }

        }
    }
}
