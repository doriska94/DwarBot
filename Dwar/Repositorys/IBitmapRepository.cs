using System.Drawing;

namespace Dwar.Repositorys;

public interface IBitmapRepository
{
    Bitmap Get(string fileName);
}
