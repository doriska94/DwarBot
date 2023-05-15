using Dwar.Repositorys;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dwar.Services
{
    public class MouseService 
    {
        public event System.Action Refresh;
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
            Refresh?.Invoke();

            await Task.Delay(DelayAfterClickMillis);
            return true;
        }
    }
}
