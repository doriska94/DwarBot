﻿using Dwar.Repositorys;

namespace Dwar.Services;

public class BotService
{
    public bool IsRunning => _isRunning;
    private bool _isRunning;
    private FarmService _farmService;
    private FightService _fightService;
    private IFightRepository _fightRepository;
    private HpService _hpService;
    private ActionExecuteService _actionExecuteService;
    private int _count;
    private DateTime _startTime;
    private DateTime _endTime;
    private ILogService _log;
    private INotifyerService _notifyer;
    public HttpService _httpService;
    public IActionRepository _actionRepository;
    private StandartExecuteService _standartExecuteService;
    public BotService(FarmService farmService, HpService hpService,
                      FightService fightControlService,
                      IFightRepository fightRepository,
                      INotifyerService notifyer,
                      ILogService log,
                      ActionExecuteService actionExecuteService,
                      HttpService httpService,
                      IActionRepository actionRepository, 
                      StandartExecuteService standartExecuteService)
    {
        _farmService = farmService;
        _hpService = hpService;
        _fightService = fightControlService;
        _fightRepository = fightRepository;
        _notifyer = notifyer;
        _log = log;
        _actionExecuteService = actionExecuteService;
        _httpService = httpService;
        _actionRepository = actionRepository;
        _standartExecuteService = standartExecuteService;
    }

    public async Task StartAsync(Bot bot, StopBotCommand stopBot)
    {
        _notifyer.Notify("Start bot");
        if (_isRunning)
            return;
        _isRunning = true;
        _count = 0;
        _startTime = DateTime.Now;
        _endTime = DateTime.Now.AddHours(bot.FightTime);
        await Running(bot,stopBot);
    }

    private async Task Running(Bot bot, StopBotCommand stopBot)
    {
        
        var action = GetActionService(bot);
        while (_isRunning)
        {
            _count++; 
            if (bot.WaitHp)
                await _hpService.WaitFullHp(stopBot);

            await action.ExecuteAsync(stopBot);
            await Task.Delay(500);
            if(ExitCondition(bot))
                _isRunning= false;

            if (stopBot.Stop)
                _isRunning = false;
            if(_count%5 == 0)
            {
                var fight = _fightRepository.Get(bot.FightId);
                if (fight.After5FightID != Guid.Empty)
                {
                    var actionAfter5Fight = _actionRepository.Get(fight.After5FightID);
                    await _httpService.ExecuteAsync(actionAfter5Fight);
                }
            }
            
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
            case SequenceType.Divnoe:
                _actionExecuteService.SetAttack(_fightRepository.Get(bot.FightId));
                return _actionExecuteService;
            case SequenceType.Execute:
                _standartExecuteService.SetAttack(_fightRepository.Get(bot.FightId));
                return _standartExecuteService;
            default:
                throw new InvalidOperationException("Type Not Found");
        }
    }
    public void Stop()
    {
        _isRunning = false;
    }
}
