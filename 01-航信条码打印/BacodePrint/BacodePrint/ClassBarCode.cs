using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BacodePrint
{
    internal class ClassBarCode
    {
        public static System.Drawing.Image GenerateBarCodeBitmap(string content)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            System.Drawing.Image img = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.White, 290, 120);
            return img;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Image bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);
                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public static BitmapImage GenerateBarCodeBitmapImage(string content)
        {
            System.Drawing.Image bmp = GenerateBarCodeBitmap(content);
            return BitmapToBitmapImage(bmp);
        }
    }
}
