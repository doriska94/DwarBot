using Dwar.Repositorys;

namespace Dwar.Services;

public class FarmService : IActionService, IActionSetService
{
    private HttpService _actionHttpService;
    private RefreshService _mouseService;
    private IActionRepository _actionRepository;
    private FarmEndService _farmEndService;
    private Fight? _fightConfig;
    private StartFightService _startFightControl;
    private FightControlService _fallControlService;

    public FarmService(RefreshService mouseService,
                       IActionRepository actionRepository,
                       HttpService actionHttpService,
                       FarmEndService farmEndService,
                       StartFightService startFightControl,
                       FightControlService fallControlService)
    {
        _mouseService = mouseService;
        _actionRepository = actionRepository;
        _actionHttpService = actionHttpService;
        _farmEndService = farmEndService;
        _startFightControl = startFightControl;
        _fallControlService = fallControlService;
    }
    public void SetAttack(Fight fightConfig)
    {
        _fightConfig = fightConfig;
    }

    public async Task ExecuteAsync(StopBotCommand stopBot)
    {
        if (_fightConfig == null)
        {
            throw new InvalidOperationException("Sequnce not setted");
        }

        _startFightControl.StopFight();
        
        var actionAttack = _actionRepository.Get(_fightConfig.AttackId);
        
        await _actionHttpService.ExecuteAsync(actionAttack);

        await _mouseService.ClickHunt();
        
        _farmEndService.SetStartFarm();

        await _farmEndService.WaitEnd(timeOut:120, stopBot);
        
        if(_startFightControl.IsFightStarted())
        {
            await _mouseService.GotToMainClick();
            await _fallControlService.Fight(stopBot);

            var actions = _actionRepository.GetAll(_fightConfig.StartUpActions);
            
            foreach (var action in actions)
            {
                await _actionHttpService.ExecuteAsync(action);
            }
        }

        await Task.Delay(1000);
    }
}
