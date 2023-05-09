using BarcodeLib;
using Gma.QrCodeNet.Encoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            BitmapImage bitmapImage = null;
            //BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            ////b.IncludeLabel = true; //带文字标签
            //b.Alignment = AlignmentPositions.CENTER;
            //b.LabelPosition = LabelPositions.BOTTOMCENTER;          //code的显示位置
            //b.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp; //图片格式
            //Font font = new Font("Arial", 20);                      //字体设置
            //b.LabelFont = font;

            var c = p_Image as FrameworkElement;

            System.Drawing.Image bmp = null;
            //if ((c.Width > 20) && (c.Height > 20))
            if ((c.Width > 0) && (c.Height > 0))
            {
                //bmp = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.White, (int)c.Width, (int)c.Height);
                using (BarcodeLib.Barcode b = new Barcode()
                {
                    Alignment = AlignmentPositions.CENTER,

                    //IncludeLabel = true,                            //带文字标签
                    //LabelFont = new Font("Arial", 20),              //字体设置
                    //LabelPosition = LabelPositions.BOTTOMCENTER,    //code的显示位置

                    ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp, //图片格式
                    //Width = (int)c.Width,
                    //Height = (int)c.Height,
                    //Width = 185,
                    Width = content.Length * 185 /12,
                    Height = 25,
                    RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                })
                {
                    try
                    {
                        bmp = b.Encode(TYPE.CODE39, content);
                        //bmp = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), (int)c.Width, (int)c.Height);
                        //bmp = b.Encode(BarcodeLib.TYPE.CODE39Extended, content, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), (int)c.Width, (int)c.Height);
                        bitmapImage = BitmapToBitmapImage(bmp);
                    }
                    catch (Exception ex)
                    {
                        string msg = "GenerateBarCode" + ex.Message;
                        Debug.Print(msg);
                        bitmapImage = (BitmapImage)p_Image.Source;
                    }
                    //finally
                    //{
                    //    Debug.Print("GenerateBarCode finally!");
                    //    bitmapImage = (BitmapImage)p_Image.Source;
                    //}
                }

            }
            else
            {
                Debug.Print("Image size is error!");
                bitmapImage = (BitmapImage)p_Image.Source;
            }

            return bitmapImage;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Image bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
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
