using Dwar.Repositorys;

namespace Dwar.Services;

public class FightControlService 
{
    public const string Victory = "fightover_victory.ogg";
    public const string Defeat = "fightover_defeat.ogg";
    public const string StartFight = "combo.css";

    private StartFightService _startFightService;
    private IUserInputService _userInput;
    private FightState _state;
    private IComboRepository _comboRepository;
    private ILogService _log;
    private INotifyerService _notifyer;
    private ITimeOutRepository _timeOutRepository;
    public FightControlService(StartFightService startFightService,
                               IUserInputService userInput,
                               IComboRepository comboRepository,
                               ILogService log,
                               INotifyerService notifyer,
                               ITimeOutRepository timeOutRepository)
    {
        _startFightService = startFightService;
        _userInput = userInput;
        _comboRepository = comboRepository;
        _log = log;
        _notifyer = notifyer;
        _timeOutRepository = timeOutRepository;
    }

    public async Task Fight(StopBotCommand stopBot)
    {
        var timeOut = _timeOutRepository.Get();
        var combo = _comboRepository.Get();
        combo.Reset();
        _notifyer.Notify("Combo reset");
        if (combo.FightInDefence)
        {
            
            await _startFightService.WaitCannAttackAsync(stopBot);
            await _startFightService.SetFocus();

            if (stopBot.Stop)
                return;

            _userInput.Left();
            timeOut.HandleAction();
        }
        if (combo.UseSkill)
        {

            await _startFightService.WaitCannAttackAsync(stopBot);
            await _startFightService.SetFocus();

            if (stopBot.Stop)
                return;

            _userInput.PressT();
            timeOut.HandleAction();
        }

        while (_startFightService.FightFinish() == false)
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
            
            timeOut.HandleAction();

            while (await _startFightService.CannAttackAsync() && _startFightService.FightFinish() == false)
            {
                if (timeOut.IsOut())
                {
                    _state = FightState.Winn;
                    return;
                }

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

                await Task.Delay(300);
            }
        }
    }

    public enum FightState
    {
        Running = 0,
        Winn = 1,
        Lose = 2,
    }        
}
