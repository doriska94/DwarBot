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

    public StartFightService(IBitmapRepository bitmapRepository, IScreenshot screenshot)
    {
        _bitmapRepository = bitmapRepository;
        _screenshot = screenshot;
    }

    public async Task WaitCannAttackAsync(StopBotCommand stopBot)
    {
        while (await CannAttackAsync() == false)
        {
            if (FightFinish())
                return;
            await Task.Delay(_waitStartFight);
            if (stopBot.Stop)
                throw new TaskCanceledException("User cancel");
        }
    }
    
    public async Task SetFocus()
    {
        var point = await GetTemplatePositionAsync();
        if(point != Point.Empty)
            Mouse.MousClick(new Mouse.MousePoint(point.X, point.Y));
    }


    public async Task<bool> CannAttackAsync()
    {
        return await GetTemplatePositionAsync() != Point.Empty;
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
    private bool FightFinish()
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
