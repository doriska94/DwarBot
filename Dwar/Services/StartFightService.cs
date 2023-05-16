using Dwar.Repositorys;
using System.Drawing;
using static Dwar.Services.FightControlService;

namespace Dwar.Services;

public class StartFightService : IHandleFightState
{
    private const string Template = "can_atack.png";
    private const int WaitStartFight = 500;
    private FightState _state;
    private IScreenshot _screenshot;
    private IBitmapRepository _bitmapRepository;

    public StartFightService(IBitmapRepository bitmapRepository, IScreenshot screenshot)
    {
        _bitmapRepository = bitmapRepository;
        _screenshot = screenshot;
    }

    public async Task WaitCannAttackAsync()
    {
        while (IsFightStarted() == false)
        {
            await Task.Delay(WaitStartFight);
        }
    }


    public bool IsFightStarted()
    {
        return GetTemplatePosition() != Point.Empty;
    }

    public void HandleRequest(string url)
    {
        if (url.Contains(FightControlService.Victory))
            _state = FightState.Winn;
        if (url.Contains(FightControlService.Defeat))
            _state = FightState.Lose;
        if (url.Contains(FightControlService.StartFight))
            _state = FightState.Running;
    }

    private Point GetTemplatePosition()
    {
        Bitmap screen = _screenshot.TakeScreenShot();
        var searchTemplate = _bitmapRepository.Get(Template);
        var point = screen.FindPosition(searchTemplate);
        return point;
    }

}
