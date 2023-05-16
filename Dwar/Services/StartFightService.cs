using Dwar.Repositorys;
using System.Drawing;
using static Dwar.Services.FightControlService;

namespace Dwar.Services;

public class StartFightService 
{
    private const string _template = "can_atack.png";
    private const int _waitStartFight = 500;
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
            await Task.Delay(_waitStartFight);
        }
    }


    public bool IsFightStarted()
    {
        return GetTemplatePosition() != Point.Empty;
    }

    private Point GetTemplatePosition()
    {
        Bitmap screen = _screenshot.TakeScreenShot();
        var searchTemplate = _bitmapRepository.Get(_template);
        var point = screen.FindPosition(searchTemplate);
        return point;
    }

}
