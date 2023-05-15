using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Dwar.UI.WindowsRepositries
{
    internal class ScreenshotRepository : IScreenshot
    {
        public Bitmap TakeScreenShot()
        {
            var screenWidth = (int)SystemParameters.VirtualScreenWidth;
            var screenHeight = (int)SystemParameters.VirtualScreenHeight;


            Bitmap bmp = new Bitmap((int)screenWidth, screenHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0,new System.Drawing.Size(screenWidth, screenHeight) );
            }
            return bmp;
        }

    }
}
