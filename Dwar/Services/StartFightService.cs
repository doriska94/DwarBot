using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Services
{
    public class StartFightService
    {
        private const string Template = "can_atack.png";
        private const int WaitStartFight = 500;

        private IBitmapRepository _bitmapRepository;
        private IScreenshot _screenshot;

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

        public void SetFightFocus()
        {
            Point point = GetTemplatePosition();

            Mouse.MousClick(new Mouse.MousePoint()
            {
                X = point.X,
                Y = point.Y
            });
        }


        public bool IsFightStarted()
        {
            Point point = GetTemplatePosition();

            return point != Point.Empty;

        }

        private Point GetTemplatePosition()
        {
            Bitmap screen = _screenshot.TakeScreenShot();
            var searchTemplate = _bitmapRepository.Get(Template);
            var point = screen.FindPosition(searchTemplate);
            return point;
        }
    }
}
