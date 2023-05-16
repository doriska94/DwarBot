using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dwar
{
    public static class BitmapExtensions
    {

#pragma warning disable CA1416 // Validate platform compatibility
        public static async Task<Point> FindPositionAsync(this Bitmap source, Bitmap template, int startSearchX = 0, int StartSearchY = 0, int EndSearcX = 0, int EndSearcY = 0)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (source == null || template == null)
                    return Point.Empty;

                Point point = Point.Empty;

                if (EndSearcX == 0)
                    EndSearcX = source.Width - template.Width;
                if (EndSearcY == 0)
                    EndSearcY = source.Height - template.Height;
                for (var outerX = startSearchX; outerX < EndSearcX; outerX++)
                {
                    for (var outerY = StartSearchY; outerY < EndSearcY; outerY++)
                    {
                        for (var innerX = 0; innerX < template.Width; innerX++)
                        {
                            for (var innerY = 0; innerY < template.Height; innerY++)
                            {
                                var searchColor = template.GetPixel(innerX, innerY);
                                var withinColor = source.GetPixel(outerX + innerX, outerY + innerY);
                                if (searchColor != withinColor)
                                    goto NotFound;
                            }
                        }
                        point = new System.Drawing.Point(outerX, outerY);
                        point.X += template.Width / 2;
                        point.Y += template.Height / 2;
                        return point;

                    NotFound:
                        continue;
                    }
                }
                point = Point.Empty;
                return point;
            });
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
