using Dwar.Repositorys;

namespace Dwar.Services;

public class FightControlService : IHandleFightState
{
    public const string Victory = "fightover_victory.ogg";
    public const string Defeat = "fightover_defeat.ogg";
    public const string StartFight = "combo.css";

    private StartFightService _startFightService;
    private IUserInput _userInput;
    private FightState _state;
    private IComboRepository _comboRepository;

    public FightControlService(StartFightService startFightService,
                               IUserInput userInput,
                               IComboRepository comboRepository)
    {
        _startFightService = startFightService;
        _userInput = userInput;
        _comboRepository = comboRepository;
    }

    public async Task Fight(StopBotCommand stopBot)
    {
        var combo = _comboRepository.Get();
        combo.Reset();
        if (combo.FightInDefence)
        {
            await _startFightService.WaitCannAttackAsync(stopBot);
            await _startFightService.SetFocus();
            if (stopBot.Stop)
                return;
            _userInput.Left();
        }

        while (FightFinish() == false)
        {
            await _startFightService.WaitCannAttackAsync(stopBot);
            await Task.Delay(500);
            await _startFightService.SetFocus();

            if (stopBot.Stop)
            {
                _state = FightState.Winn;
                return;
            }
            var nextStep = combo.GetNext();

            await Task.Delay(nextStep.Delay * 1000);

            while (await _startFightService.CannAttackAsync() && FightFinish() == false)
            {
                if (stopBot.Stop)
                {
                    _state = FightState.Winn;
                    return;
                }

                switch (nextStep.Type)
                {
                    case ComboStepType.Up:
                        _userInput.Up();
                        break;
                    case ComboStepType.Down:
                        _userInput.Down();
                        break;
                    case ComboStepType.Forward:
                        _userInput.Right();
                        break;
                }
            }
        }
    }
    private bool FightFinish()
    {
        return _state == FightState.Winn || _state == FightState.Lose;
    }

    public void HandleRequest(string url)
    {
        if (url.Contains(Victory))
            _state = FightState.Winn;
        if (url.Contains(Defeat))
            _state = FightState.Lose;
        if (url.Contains(StartFight))
            _state = FightState.Running;
    }
    public enum FightState
    {
        Running = 0,
        Winn = 1,
        Lose = 2,
    }        
}
