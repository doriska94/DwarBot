using Dwar.Repositorys;
using System.Drawing;
using System.Windows;

namespace Dwar.UI.WindowsRepositries;

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
