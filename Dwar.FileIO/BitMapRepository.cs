using Dwar.Repositorys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar.FileIO
{
    public class BitMapRepository : IBitmapRepository
    {
#pragma warning disable CA1416 // Validate platform compatibility
        private Dictionary<string,Bitmap> bitmaps = new();
        public Bitmap Get(string fileName)
        {
            if (bitmaps.ContainsKey(fileName))
                return bitmaps[fileName];

            Bitmap bitmap = new Bitmap(fileName);
            bitmaps.Add(fileName, bitmap);
            return bitmap;
            
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
