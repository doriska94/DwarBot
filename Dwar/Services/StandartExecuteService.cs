using Dwar.Repositorys;

namespace Dwar.Services;

public class StandartExecuteService : IActionService, IActionSetService
{
    private Fight _fight = null!;
    private HttpService _httpService;
    private IActionRepository _actionRepository;
    public StandartExecuteService(HttpService httpService, IActionRepository actionRepository)
    {
        _httpService = httpService;
        _actionRepository = actionRepository;
    }

    public async Task ExecuteAsync(StopBotCommand stopBot)
    {

        if(_fight == null)
            return;

        var actionStart = _actionRepository.Get(_fight.AttackId);

        await _httpService.ExecuteAsync(actionStart);

        var actions = _actionRepository.GetAll(_fight.StartUpActions);

        foreach (var action in actions)
        {
            await _httpService.ExecuteAsync(action);
        }
    }

    public void SetAttack(Fight fightConfig)
    {
        _fight = fightConfig;
    }
}
