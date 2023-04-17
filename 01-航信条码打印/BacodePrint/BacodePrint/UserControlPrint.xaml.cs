﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BacodePrint
{
    /// <summary>
    /// UserControlPrint.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlPrint : UserControl
    {
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        IniFile mFilesINI = new IniFile();

        List<UserControlTextBoxItems> listText = new List<UserControlTextBoxItems>();
        const int nListMaxCount = 34;

        private System.Windows.Point startPoint = new System.Windows.Point();
        //动态调整大小
        private CanvasAdorner m_CanvasAdorner = null;

        private bool isDown = false;
        private System.Windows.Shapes.Path originalElement = new System.Windows.Shapes.Path();


        public UserControlPrint()
        {
            InitializeComponent();

            Init();
        }

        //初始化
        private void Init()
        {
            for (int i = 0; i < nListMaxCount; i++)
            {
                string str;
                string strSetion = "Text" + i.ToString();

                UserControlTextBoxItems UserControlTextBoxItems = new UserControlTextBoxItems();
                var c = UserControlTextBoxItems as FrameworkElement;

                byte nColor = (byte)(i * 3);
                Brush brush = new SolidColorBrush(Color.FromArgb(0xff, 0x7f, 0x30, nColor));
                UserControlTextBoxItems.Background = brush;

                str = mFilesINI.INIRead(strSetion, "FontFamily", mSystemInfo.mstrConfigFilePath);
                UserControlTextBoxItems.FontFamily = new FontFamily(str);

                str = mFilesINI.INIRead(strSetion, "FontSize", mSystemInfo.mstrConfigFilePath);
                UserControlTextBoxItems.FontSize = Convert.ToInt32(str);

                str = mFilesINI.INIRead(strSetion, "RowSpacing", mSystemInfo.mstrConfigFilePath);
                UserControlTextBoxItems.mnRowSpacing = Convert.ToInt32(str);

                str = mFilesINI.INIRead(strSetion, "ColumnSpacing", mSystemInfo.mstrConfigFilePath);
                UserControlTextBoxItems.mnColumnSpacing = Convert.ToInt32(str);

                str = mFilesINI.INIRead(strSetion, "Left", mSystemInfo.mstrConfigFilePath);
                Canvas.SetLeft(c, Convert.ToInt32(str));

                str = mFilesINI.INIRead(strSetion, "Top", mSystemInfo.mstrConfigFilePath);
                Canvas.SetTop(c, Convert.ToInt32(str));

                str = mFilesINI.INIRead(strSetion, "Width", mSystemInfo.mstrConfigFilePath);
                c.Width = Convert.ToInt32(str);

                str = mFilesINI.INIRead(strSetion, "Height", mSystemInfo.mstrConfigFilePath);
                c.Height = Convert.ToInt32(str);
                
                listText.Add(UserControlTextBoxItems);
                canvas1.Children.Add(c);
            }
        }

        public void SaveIni()
        {
            for (int i = 0; i < listText.Count; i++)
            {
                string str;
                string strSetion = "Text" + i.ToString();
                UserControlTextBoxItems UserControlTextBoxItems = listText[i];

                var c = UserControlTextBoxItems as FrameworkElement;
                //字体
                str = UserControlTextBoxItems.FontFamily.ToString();
                mFilesINI.INIWrite(strSetion, "FontFamily", str, mSystemInfo.mstrConfigFilePath);
                //字号
                str = UserControlTextBoxItems.FontSize.ToString();
                mFilesINI.INIWrite(strSetion, "FontSize", str, mSystemInfo.mstrConfigFilePath);
                //行间距
                str = UserControlTextBoxItems.mnRowSpacing.ToString();
                mFilesINI.INIWrite(strSetion, "RowSpacing", str, mSystemInfo.mstrConfigFilePath);
                //列间距
                str = UserControlTextBoxItems.mnColumnSpacing.ToString();
                mFilesINI.INIWrite(strSetion, "ColumnSpacing", str, mSystemInfo.mstrConfigFilePath);
                //Left
                str = Canvas.GetLeft(c).ToString();
                mFilesINI.INIWrite(strSetion, "Left", str, mSystemInfo.mstrConfigFilePath);
                //TOP
                str = Canvas.GetTop(c).ToString();
                mFilesINI.INIWrite(strSetion, "Top", str, mSystemInfo.mstrConfigFilePath);
                //Width
                str = c.Width.ToString();
                mFilesINI.INIWrite(strSetion, "Width", str, mSystemInfo.mstrConfigFilePath);
                //Height;
                str = c.Height.ToString();
                mFilesINI.INIWrite(strSetion, "Height", str, mSystemInfo.mstrConfigFilePath);
            }
        }

        public void SetPrintBarcode(string strCode)
        {
            BitmapImage tmpImage = ClassBarCode.GenerateBarCodeBitmapImage(strCode);
            this.BarcodeImage.Source = tmpImage;

            Test.itemCtrl.FontSize = 10;
            Test.itemCtrl.FontFamily = new FontFamily("STSong");
            Test.SetString("fdsf小贩之歌刘功臣!!CXVf,.;'po");
            Test.SetSpacing(10, 2);

            AddCtrlList();
        }

        public void AddCtrlList()
        {
            //int nLeft = 30;
            //int nTop = 30;

            //for (int i = 0; i < nListMaxCount; i++)
            //{
            //    nLeft += 20;
            //    nTop += 20;
            //    UserControlTextBoxItems UserControlTextBoxItems = new UserControlTextBoxItems();
            //    Brush brush = new SolidColorBrush(Color.FromArgb(0xff, 0x7f, 0x30, 0x78));
            //    UserControlTextBoxItems.Background = brush;
            //    var c = UserControlTextBoxItems as FrameworkElement;
            //    Canvas.SetLeft(c, nLeft);
            //    Canvas.SetTop(c, nTop);
            //    c.Width = 200;
            //    c.Height = 50;

            //    UserControlTextBoxItems.Visibility = Visibility.Visible;
            //    listText.Add(UserControlTextBoxItems);
            //    canvas1.Children.Add(c);
            //}

          

        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!canvas1.IsMouseCaptured)
            {
                startPoint = e.GetPosition(canvas1);
                canvas1.CaptureMouse();
                if ((e.Source) is System.Windows.Shapes.Path path)
                {
                    if (canvas1 == e.Source)
                    {
                        return;
                    }
                    
                    isDown = true;
                    originalElement = path;
                    e.Handled = true;

                    m_CanvasAdorner.SetPosition((int)path.Data.Bounds.Left, (int)path.Data.Bounds.Top, (int)path.Data.Bounds.Width, (int)path.Data.Bounds.Height);
                    m_CanvasAdorner.SetVisibleState(Visibility.Visible);

                    //SetBindingDataToParameter(m_CanvasAdorner.MPath);
                    m_CanvasAdorner.MPath = path;
                    //GetBindingData(path);
                    //VisibleAreaParameter();

                }

                if ((e.Source) is UserControlTextBoxItems textBox)
                {

                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!canvas1.IsMouseCaptured)
            {
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!canvas1.IsMouseCaptured)
            {
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(border);
            m_CanvasAdorner = new CanvasAdorner(border);
            layer.Add(m_CanvasAdorner);
            m_CanvasAdorner.SetVisibleState(Visibility.Hidden);

            ReSize();
        }

        private void ReSize()
        {
            //outside.Width = ShowImage.Width;
            //if (((int)GridShowImage.ActualHeight != 0) && ((int)GridShowImage.ActualWidth != 0))
            //{
            //    double nHeight = GridShowImage.ActualWidth * nImageHeight / nImageWidth;
            //    double nWidth = GridShowImage.ActualHeight * nImageWidth / nImageHeight;

            //    //如果高度小于当前网格高度，说明宽度正确不用调整
            //    if (nHeight < GridShowImage.ActualHeight)
            //    {
            //        outside.Height = nHeight;
            //        outsideTest.Height = nHeight;
            //    }

            //    if (nWidth < GridShowImage.ActualWidth)
            //    {
            //        outside.Width = nWidth;
            //        outsideTest.Width = nWidth;
            //    }
            //}

        }
    }
}
