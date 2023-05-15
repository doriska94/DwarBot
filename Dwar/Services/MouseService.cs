using Dwar.Repositorys;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dwar.Services
{
    public class MouseService 
    {
        private const string TemplateName = "ohota.png";
        private const int DelayAfterClickMillis = 500;

        private IBitmapRepository _bitmapRepository;
        private IScreenshot _screenshot;
        private Point _huntPoint;
        public MouseService(IBitmapRepository bitmapRepository, IScreenshot screenshot)
        {
            _bitmapRepository = bitmapRepository;
            _screenshot = screenshot;
        }

        public async Task<bool> ClickHunt()
        {
            bool reslut = false;
            if (_huntPoint == Point.Empty)
            {
                Bitmap screen = _screenshot.TakeScreenShot();
                var searchTemplate = _bitmapRepository.Get(TemplateName);
                _huntPoint = screen.FindPosition(searchTemplate);
            }

            Mouse.MousClick(new Mouse.MousePoint() 
            { 
                X = _huntPoint.X, 
                Y = _huntPoint.Y 
            });

            await Task.Delay(DelayAfterClickMillis);
            return reslut;
        }
    }
}
