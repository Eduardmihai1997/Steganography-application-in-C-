using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageSecret.Services.SteganographyService
{

    // LEARN ABOUT INTERFACES HERE
    public interface ISteganographyService
    {
        BitmapImage Encode(BitmapImage image, string message);
        string Decode(BitmapImage image);
    }
}
