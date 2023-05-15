using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.Repositorys
{
    public interface IBitmapRepository
    {
        Bitmap Get(string fileName);
    }
}
