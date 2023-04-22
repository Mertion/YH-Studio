using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public static BitmapImage GenerateBarCodeBitmapImage(string content)
        {
            System.Drawing.Image bmp = GenerateBarCodeBitmap(content);
            return BitmapToBitmapImage(bmp);
        }

        public static BitmapImage GenerateBarCodeBitmap(string content, ref System.Windows.Controls.Image p_Image)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            //b.IncludeLabel = true; //带文字标签
            b.Alignment = AlignmentPositions.CENTER;
            b.LabelPosition = LabelPositions.BOTTOMCENTER;          //code的显示位置
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp; //图片格式
            Font font = new Font("Arial", 20);                      //字体设置
            b.LabelFont = font;

            var c = p_Image as FrameworkElement;
            
            //b.BarWidth = (int)Pa.RenderSize.Width; //自动调整条形码宽度
            System.Drawing.Image bmp = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.White, (int)c.Width, (int)c.Height);
            //img.Save("Barcode.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return BitmapToBitmapImage(bmp);
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
    }
}
