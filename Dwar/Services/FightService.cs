using Dwar.Repositorys;

namespace Dwar.Services;

public class FightService : IActionService, IActionSetService
{
    private HttpService _actionHttpService;
    private RefreshService _mouseService;
    private StartFightService _startFightService;
    private FightControlService _fightControlService;
    private Fight? _fightConfig;
    private IActionRepository _actionRepository;
    private ILogService _log;
    private INotifyerService _notifyer;
    private ITimeOutRepository _timeOutRepository;
    public FightService(HttpService actionHttpService,
                              RefreshService mouseService,
                              StartFightService startFightService,
                              FightControlService fightControlService,
                              IActionRepository actionRepository,
                              ILogService log,
                              INotifyerService notifyer,
                              ITimeOutRepository timeOutRepository)
    {
        _actionHttpService = actionHttpService;
        _mouseService = mouseService;
        _startFightService = startFightService;
        _fightControlService = fightControlService;
        _actionRepository = actionRepository;
        _log = log;
        _notifyer = notifyer;
        _timeOutRepository = timeOutRepository;
    }
    public void SetAttack(Fight fightConfig)
    {
        _fightConfig = fightConfig;
    }

    public async Task ExecuteAsync(StopBotCommand stopBot)
    {
        var timeOut = _timeOutRepository.Get();

        if (_fightConfig == null)
        {
            throw new InvalidOperationException("Attack not setted");
        }
        _startFightService.StopFight();
        _notifyer.Notify("Attack");
        timeOut.HandleAction();
        while (await _actionHttpService.ExecuteAsync(_actionRepository.Get(_fightConfig.AttackId)) == false)
        {
            if(timeOut.IsOut())
            {
                return;
            }
            await Task.Delay(100);
            if (stopBot.Stop)
            {
                return;
            }
        }

        _startFightService.OnStartFight();
        
        if(timeOut.IsOut() == false)
            timeOut.HandleAction();

        var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
        
        _notifyer.Notify("Call extra actions");

        foreach (var action in actions)
        {
            while( await _actionHttpService.ExecuteAsync(action) == false)
            {
                if(timeOut.IsOut())
                    break;

                await Task.Delay(100);
                if (stopBot.Stop)
                    return;
            }

            if (timeOut.IsOut())
                break;
            else
                timeOut.HandleAction();
        }

        await _startFightService.WaitCannAttackAsync(stopBot); //wait start bot => screen analyse

        await _fightControlService.Fight(stopBot); //fight => screen analyse
    }
}
