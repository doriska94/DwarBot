using Dwar.Repositorys;
using System.Drawing;
using static Dwar.Services.FightControlService;

namespace Dwar.Services;

public class StartFightService : IHandleFightState
{
    private const string _template = "can_atack.png";
    private const int _waitStartFight = 500;
    private IScreenshot _screenshot;
    private FightState _state;
    private IBitmapRepository _bitmapRepository;
    private INotifyer _notifyer;
    public ILog _log;
    public ITimeOutRepository _timeOut;
    public StartFightService(IBitmapRepository bitmapRepository, IScreenshot screenshot, INotifyer notifyer, ILog log, ITimeOutRepository timeOut)
    {
        _bitmapRepository = bitmapRepository;
        _screenshot = screenshot;
        _notifyer = notifyer;
        _log = log;
        _timeOut = timeOut;
    }

    public async Task WaitCannAttackAsync(StopBotCommand stopBot)
    {
        _notifyer.Notify("Wait cann attack");
        var timeOut = _timeOut.Get();
        while (await CannAttackAsync() == false)
        {
            if (FightFinish())
                return;
            if (timeOut.IsOut())
                return;
            await Task.Delay(_waitStartFight);
            if (stopBot.Stop)
            {
                _state = FightState.Winn;
                throw new TaskCanceledException("User cancel");
            }
        }
        timeOut.HandleAction();
    }

    public async Task SetFocus()
    {
        var timeOut = _timeOut.Get();
        _notifyer.Notify("Set focus on attack");

        var point = await GetTemplatePositionAsync();

        await _log.Write("Image point: ", point.ToString());

        if(point != Point.Empty)
            Mouse.MousClick(new Mouse.MousePoint(point.X, point.Y));

        timeOut.HandleAction();
    }


    public async Task<bool> CannAttackAsync()
    {
        var point = await GetTemplatePositionAsync();
        await _log.Write("Image point: ", point.ToString());
        return point != Point.Empty;
    }

    private async Task<Point> GetTemplatePositionAsync()
    {
        Bitmap screen = _screenshot.TakeScreenShot();
        var searchTemplate = _bitmapRepository.Get(_template);
        var point = await screen.FindPositionAsync(searchTemplate, 380, 300, 540, 620);
        
        return point;
    }
    public void StopFight()
    {
        _state = FightState.Winn;
    }
    public void OnStartFight()
    {
        _state = FightState.Running;
    }
    public bool FightFinish()
    {
        return _state == FightState.Winn || _state == FightState.Lose;
    }

    public bool IsFightStarted()
    {
        return _state == FightState.Running;
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
}
