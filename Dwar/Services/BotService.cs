using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class BotService
    {
        public bool IsRunning => _isRunning;
        private bool _isRunning;
        private FarmService _farmService;
        private FightService _fightService;
        private IFightRepository _fightRepository;
        private HpService _hpService;
        private int _count;
        private DateTime _startTime;
        private DateTime _endTime;

        public BotService(FarmService farmService, HpService hpService, FightService fightControlService, IFightRepository fightRepository)
        {
            _farmService = farmService;
            _hpService = hpService;
            _fightService = fightControlService;
            _fightRepository = fightRepository;
        }

        public async Task StartAsync(Bot bot)
        {
            if (_isRunning)
                return;
            _isRunning = true;
            _count = 0;
            _startTime = DateTime.Now;
            _endTime = DateTime.Now.AddHours(bot.FightTime);
            await Running(bot);
        }

        private async Task Running(Bot bot)
        {
            
            var action = GetActionService(bot);
            while (!_isRunning)
            {
                if (bot.WaitHp)
                    await _hpService.WaitFullHp();

                await action.ExecuteAsync();

                if(ExitCondition(bot))
                    _isRunning= false;
            }
        }

        public bool ExitCondition(Bot bot)
        {
            return (_count >= bot.FightCount && bot.FightCount != 0) ||
                   (_startTime >= _endTime && bot.FightTime != 0);
        }

        private IActionService GetActionService(Bot bot)
        {

            switch (bot.Type)
            {
                case SequenceType.Fight:
                    _fightService.SetAttack(_fightRepository.Get(bot.FightId));
                    return _fightService;
                case SequenceType.Farm:
                    _farmService.SetAttack(_fightRepository.Get(bot.FightId));
                    return _farmService;
                default:
                    throw new InvalidOperationException("Type Not Found");
            }
        }
        public void Stop()
        {
            _isRunning = false;
        }
    }
}
