using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class BotService
    {
        private bool _isRunning;
        private IActionService _actionService;
        private HpService _hpService;

        public BotService(IActionService actionService, HpService hpService)
        {
            _actionService = actionService;
            _hpService = hpService;
        }

        public async void StartAsync(Bot bot)
        {
            _isRunning = true;
            
            await Running(bot);
            
        }

        private async Task Running(Bot bot)
        {
            while (!_isRunning)
            {
                if (bot.WaitHp)
                    await _hpService.WaitFullHp();
                await _actionService.ExecuteAsync();
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}
