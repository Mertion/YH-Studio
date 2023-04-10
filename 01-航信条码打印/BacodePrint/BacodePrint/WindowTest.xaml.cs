using BarcodeLib;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;

//using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//using System.Windows.Shapes;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
//using System.Windows.Markup;
//using System.Windows.Navigation;

using FontFamily = System.Windows.Media.FontFamily;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Windows.Shell;
//using static System.Net.Mime.MediaTypeNames;

namespace BacodePrint
{
    /// <summary>
    /// WindowTest.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTest : Window
    {
        
        public WindowTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image bmp = GenerateBarCodeBitmap("5381921979 0", ref this.BarcodeImage);
            BitmapImage tmpImage = BitmapToBitmapImage(bmp);
            this.BarcodeImage.Source = tmpImage;
            
        }


        public static System.Drawing.Image GenerateBarCodeBitmap(string content, ref System.Windows.Controls.Image p_Image)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            //System.Drawing.Image img = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.White, 290, 120);


            b.IncludeLabel = true; //带文字标签
            b.Alignment = AlignmentPositions.CENTER;
            b.LabelPosition = LabelPositions.BOTTOMCENTER;          //code的显示位置
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp; //图片格式
            Font font = new Font("Arial", 20);                      //字体设置
            b.LabelFont = font;

            System.Windows.Controls.Border Pa = (System.Windows.Controls.Border)p_Image.Parent;
            //b.BarWidth = (int)Pa.RenderSize.Width; //自动调整条形码宽度
            System.Drawing.Image img = b.Encode(BarcodeLib.TYPE.CODE39, content, System.Drawing.Color.Black, System.Drawing.Color.White, (int)Pa.RenderSize.Width, (int)Pa.RenderSize.Height);
            //img.Save("Barcode.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            //QrCode qrCode = qrEncoder.Encode("www.baidu.com");

            //QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            //QrCode qrCode = new QrCode();
            //qrEncoder.TryEncode("www.baidu.com", out qrCode);

            //Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
            //MemoryStream ms = new MemoryStream();
            //renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Bmp);

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode("www.baidu.com", out qrCode);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), System.Drawing.Brushes.Black, System.Drawing.Brushes.White);

            using (MemoryStream ms = new MemoryStream())
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Bmp, ms);
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = ms;
                result.EndInit();
                result.Freeze();
                this.BarcodeImage.Source = result;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                PrintPage tPrintPage = new PrintPage();
                tPrintPage.Width = 1131;
                tPrintPage.Height = 479;
                //tPrintPage.SetPrintBarcode("5381921979 0");

                string str = "5381921979 " + i.ToString();
                tPrintPage.getPrinter().SetPrintBarcode(str);
                //tPrintPage.ShowDialog();

                tPrintPage.PrintWindows();

                tPrintPage.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FillFontComboBox(this.FontCombox);

        }

        public void FillFontComboBox(ComboBox comboBoxFonts)
        {
            // Enumerate the current set of system fonts,
            // and fill the combo box with the names of the fonts.
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                comboBoxFonts.Items.Add(fontFamily.Source);
            }

            comboBoxFonts.SelectedIndex = 0;
        }

        //private void InitializeFontFamilyList()
        //{
        //    ICollection<FontFamily> familyCollection = Fonts.SystemFontFamilies;
        //    //ICollection<FontFamily> familyCollection = Fonts.SystemFontFamilies;
        //    if (familyCollection != null)
        //    {
        //        FontFamilyListItem[] items = new FontFamilyListItem[familyCollection.Count];
        //        //FontFamilyListItem[] items = new FontFamilyListItem[familyCollection.Count];
        //        int i = 0;

        //        foreach (FontFamily family in familyCollection)
        //        {
        //            items[i++] = new FontFamilyListItem(family);
        //        }

        //        Array.Sort<FontFamilyListItem>(items);

        //        foreach (FontFamilyListItem item in items)
        //        {
        //            // 加入你的处理代码
        //        }
        //    }
        //}

        private Thickness margin = new Thickness(5);
        public Thickness Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        private string title2 = "测试字体间距zjis";
        public string Title2
        {
            get { return title2; }
            set
            {
                title2 = value;
                OnPropertyChanged("Title2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FontItem tFontItem = new FontItem();
            WindowFontList dlg = new WindowFontList();

            dlg.mFontItem = tFontItem;
            dlg.ShowDialog();

            itemCtrl.FontFamily = new FontFamily(tFontItem.Name);
            itemCtrl.FontSize = 50;

            Title2 = tFontItem.Name + "   测试中文点阵字";

            //itemCtrl.Items.Clear();
            itemCtrl.ItemsSource = title2;
            int x = itemCtrl.ItemBindingGroup.Items.Count;

            Debug.Print(x.ToString());

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>
            {
                "Item #1",
                "Item #1",
                "Item #1",
                "Item #1"
            };
            if(itemCtrlCmd.Items.Count>0)
            {
                itemCtrlCmd.Items.Clear();
            }
            itemCtrlCmd.ItemsSource = items;
        }

        //要继承 INotifyPropertyChanged接口
    }
}
