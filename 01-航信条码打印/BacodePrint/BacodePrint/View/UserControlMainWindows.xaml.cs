using BacodePrint.View;
using Gma.QrCodeNet.Encoding.Masking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Shell;
using System.Windows.Threading;

namespace BacodePrint
{
    /// <summary>
    /// 字体结构体
    /// </summary>
    public class FontItem
    {
        /// <summary>
        /// 字体名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// UserControlMainWindows.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMainWindows : UserControl
    {
        double m_dOffsetX = 0.0;
        double m_dOffsetY = 0.0;

        //页面尺寸

        double m_dWidth = 793.70078740157476;
        double m_dHeight = 1122.5196850393702;

        //字体列表
        private ObservableCollection<FontItem> fontItemList = new ObservableCollection<FontItem>();

        Template mTemplate = new Template();
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        TemplateFundation mTemplateFundation = new TemplateFundation();

        List<string> mListText = new List<string>();

        //System.Windows.Point previousPoint;
        //bool isTranslateStart = false;
        private System.Windows.Point startPoint = new System.Windows.Point();
        //动态调整大小
        private CanvasAdorner m_CanvasAdorner = null;
        System.Windows.Point currentPoint = new System.Windows.Point();
        private bool isDown = false;
        private System.Windows.Shapes.Path originalElement = new System.Windows.Shapes.Path();
        private System.Windows.Shapes.Path movingElement = new System.Windows.Shapes.Path();
        FrameworkElement movingElementCtrl = null;
        private bool isDragging = false;

        private SolidColorBrush selectFillColor = new SolidColorBrush();
        private SolidColorBrush selectBorderColor = new SolidColorBrush();

        BitmapImage bitmap = new BitmapImage();

        WindowExcel windowExcel = new WindowExcel();

        //DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public UserControlMainWindows()
        {
            InitializeComponent();

            bitmap.BeginInit();
            //bitmap.UriSource = new Uri(@"C:\Users\WPF加载图片文件\WpfApp1\Images\VS2015.png");
            bitmap.UriSource = new Uri(mSystemInfo.mstrItineraryFilePath);
            bitmap.EndInit();


            selectFillColor.Color = Colors.LightCoral;
            selectFillColor.Opacity = 0.5;
            selectBorderColor.Color = Colors.Red;

            //更新字体列表
            GetLocalItem();
            this.FontCombox.ItemsSource = fontItemList;
            FontComboxEN.ItemsSource = fontItemList;
            FontComboxOrderNumber.ItemsSource = fontItemList;

            ReadFont();

            //初始化字号列表和其它配置信息
            {
                for (int i = 0; i < 50; i++)
                {
                    FontSizeCH.Items.Add(i.ToString());
                    FontSizeNumber.Items.Add(i.ToString());
                    FontWordSpacing.Items.Add(i.ToString());
                    RowWordSpacing.Items.Add(i.ToString());
                    FontSizeOrderNumber.Items.Add(i.ToString());
                    FontWordSpacingOrderNumber.Items.Add((i + 1).ToString());
                    GeneralRowWordSpacing.Items.Add((i + 1).ToString());
                }

                FontAlignment.Items.Add("左对齐");
                FontAlignment.Items.Add("右对齐");
                FontAlignment.SelectedIndex = 0;
                ReadConfig();
            }
            double nWidth = 0;
            double nHeight= 0;

            int nLeft = 0;
            int nTop = 0;

            string str = "";
            mListText.Clear();

            str = "5381921979 0  ";
            mListText.Add(str);
            str = "姓名";
            mListText.Add(str);
            str = "300225199610023865";
            mListText.Add(str);
            str = "不得签转/变更退票收费";
            mListText.Add(str);
            str = "JWM6NN";
            mListText.Add(str);

            str = "起T1";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";

            mListText.Add(str);
            str = "昆明-长水";
            mListText.Add(str);
            str = "重庆-江北";
            mListText.Add(str);
            str = "VOID";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            
            mListText.Add(str);
            str = "到T4";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "南航";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "CZ6951";
            mListText.Add(str);
            str = "VOID";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "L";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "2023-01-17";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "16：43";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "LRE9W";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "18JAN";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "18JAN";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "22K";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);
            str = "";
            mListText.Add(str);

            str = "票价CNY";
            mListText.Add(str);
            str = "票价000.00";
            mListText.Add(str);
            str = "基建CNY";
            mListText.Add(str);
            str = "基建000.00";
            mListText.Add(str);
            str = "燃油CNY";
            mListText.Add(str);
            str = "燃油000.00";
            mListText.Add(str);
            str = "税费CNY";
            mListText.Add(str);
            str = "税费000.00";
            mListText.Add(str);
            str = "合计CNY";
            mListText.Add(str);
            str = "合计000.00";
            mListText.Add(str);
            str = "7841198944315";
            mListText.Add(str);
            str = "1998";
            mListText.Add(str);
            str = "提示信息";
            mListText.Add(str);
            str = "XXX";
            mListText.Add(str);
            str = "CTU555";
            mListText.Add(str);
            str = "08333317";
            mListText.Add(str);

            str = "成都誉晨商务服务有限公司";
            mListText.Add(str);
            str = "2023-01-24";
            mListText.Add(str);

            mTemplateFundation.LoadTemplateToCanvas(mTemplate, ref this.canvas1);
            mTemplateFundation.LoadTemplateFromIni(mSystemInfo.mstrConfigFilePath, ref mTemplate, ref this.canvas1
                , ref nWidth, ref nHeight, ref m_dOffsetX, ref m_dOffsetY);
            mTemplateFundation.SetTemplateData("5381921979 0", mListText, ref mTemplate);
            mTemplateFundation.SetBorderThickness(ref mTemplate, 2);
            HiddenTextBoxItems();

            m_dWidth = Math.Round(nWidth, 2) * SystemGlobalInfo.const_dScale;
            m_dHeight = Math.Round(nHeight, 2) * SystemGlobalInfo.const_dScale;
            this.canvas1.Width = m_dWidth;
            this.canvas1.Height = m_dHeight;

            Canvas.SetLeft(ShowImage, 0);
            Canvas.SetTop(ShowImage, 0);
            ShowImage.Width = m_dWidth;
            ShowImage.Height = m_dHeight;

            ShowImage.Source = bitmap;

            PrintWidth.Text = Math.Round(nWidth, 2).ToString();
            PrintHeight.Text = Math.Round(nHeight, 2).ToString();
            PrintX.Text = Math.Round(m_dOffsetX, 2).ToString();
            PrintY.Text = Math.Round(m_dOffsetY, 2).ToString();

            nLeft =(int)( (this.DockPanelOutSide.Width - m_dWidth) /2);
            nTop = (int)((this.DockPanelOutSide.Height - m_dHeight) /2);
            Canvas.SetLeft(this.canvas1, nLeft);
            Canvas.SetTop(this.canvas1, nTop);

            //dispatcherTimer.Tick += new EventHandler(timer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(1000000);
            //dispatcherTimer.Start();
            //dispatcherTimer.Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //mTemplateFundation.GenerateBarCode("5381921979 0", ref mTemplate);
        }

        void ReadConfig()
        {
            IniFile tFilesINI = new IniFile();

            string str = tFilesINI.INIRead("Config", "FontSizeCH", mSystemInfo.mstrConfigFilePath);
            FontSizeCH.Text = str;

            str = tFilesINI.INIRead("Config", "FontSizeNumber", mSystemInfo.mstrConfigFilePath);
            FontSizeNumber.Text = str;

            str = tFilesINI.INIRead("Config", "FontWordSpacing", mSystemInfo.mstrConfigFilePath);
            FontWordSpacing.Text = str;

            str = tFilesINI.INIRead("Config", "RowWordSpacing", mSystemInfo.mstrConfigFilePath);
            RowWordSpacing.Text = str;

            str = tFilesINI.INIRead("Config", "GeneralRowWordSpacing", mSystemInfo.mstrConfigFilePath);
            GeneralRowWordSpacing.Text= str;

            str = tFilesINI.INIRead("Config", "FontSizeOrderNumber", mSystemInfo.mstrConfigFilePath);
            FontSizeOrderNumber.Text = str;

            str = tFilesINI.INIRead("Config", "FontWordSpacingOrderNumber", mSystemInfo.mstrConfigFilePath);
            FontWordSpacingOrderNumber.Text = str;

            str = tFilesINI.INIRead("Config", "IsFontWeight", mSystemInfo.mstrConfigFilePath);
            CheckBoldFont.IsChecked = str != "0";

            str = tFilesINI.INIRead("Config", "IsFontWeightNumber", mSystemInfo.mstrConfigFilePath);
            CheckBoldFontOrderNumber.IsChecked = str != "0";

            str = tFilesINI.INIRead("Config", "PrintNumber", mSystemInfo.mstrConfigFilePath);
            CheckPrintOrderNumber.IsChecked = str != "0";

            str = tFilesINI.INIRead("Config", "PrintBarcode", mSystemInfo.mstrConfigFilePath);
            CheckPrintBarcode.IsChecked = str != "0";

            if ((bool)CheckPrintOrderNumber.IsChecked)
            {
                mTemplate.listText[0].Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.listText[0].Visibility = Visibility.Hidden;
            }

            if ((bool)CheckPrintBarcode.IsChecked)
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Hidden;
            }

            str = tFilesINI.INIRead("Config", "Alignment", mSystemInfo.mstrConfigFilePath);
            FontAlignment.SelectedIndex = Convert.ToInt32(str);

            SetFontAlignment(FontAlignment.SelectedIndex);
        }

        void SaveConfig()
        {
            IniFile tFilesINI = new IniFile();

            tFilesINI.INIWrite("Config", "FontSizeCH", FontSizeCH.Text, mSystemInfo.mstrConfigFilePath);
            tFilesINI.INIWrite("Config", "FontSizeNumber", FontSizeNumber.Text, mSystemInfo.mstrConfigFilePath);
            tFilesINI.INIWrite("Config", "FontWordSpacing", FontWordSpacing.Text, mSystemInfo.mstrConfigFilePath);
            tFilesINI.INIWrite("Config", "RowWordSpacing", RowWordSpacing.Text, mSystemInfo.mstrConfigFilePath);

            tFilesINI.INIWrite("Config", "GeneralRowWordSpacing", GeneralRowWordSpacing.Text, mSystemInfo.mstrConfigFilePath);
            
            tFilesINI.INIWrite("Config", "FontSizeOrderNumber", FontSizeOrderNumber.Text, mSystemInfo.mstrConfigFilePath);
            tFilesINI.INIWrite("Config", "FontWordSpacingOrderNumber", FontWordSpacingOrderNumber.Text, mSystemInfo.mstrConfigFilePath);

            if ((bool)CheckBoldFont.IsChecked)
            {
                tFilesINI.INIWrite("Config", "IsFontWeight", "1", mSystemInfo.mstrConfigFilePath); 
            }
            else
            {
                tFilesINI.INIWrite("Config", "IsFontWeight", "0", mSystemInfo.mstrConfigFilePath);
            }

            if ((bool)CheckBoldFontOrderNumber.IsChecked)
            {
                tFilesINI.INIWrite("Config", "IsFontWeightNumber", "1", mSystemInfo.mstrConfigFilePath);
            }
            else
            {
                tFilesINI.INIWrite("Config", "IsFontWeightNumber", "0", mSystemInfo.mstrConfigFilePath);
            }

            if ((bool)CheckPrintOrderNumber.IsChecked)
            {
                tFilesINI.INIWrite("Config", "PrintNumber", "1", mSystemInfo.mstrConfigFilePath);
            }
            else
            {
                tFilesINI.INIWrite("Config", "PrintNumber", "0", mSystemInfo.mstrConfigFilePath);
            }

            if ((bool)CheckPrintBarcode.IsChecked)
            {
                tFilesINI.INIWrite("Config", "PrintBarcode", "1", mSystemInfo.mstrConfigFilePath);
            }
            else
            {
                tFilesINI.INIWrite("Config", "PrintBarcode", "0", mSystemInfo.mstrConfigFilePath);
            }

            if(FontAlignment.SelectedIndex==0)
            {
                tFilesINI.INIWrite("Config", "Alignment", "0", mSystemInfo.mstrConfigFilePath);
            }
            else
            {
                tFilesINI.INIWrite("Config", "Alignment", "1", mSystemInfo.mstrConfigFilePath);
            }
        }

        void ReadFont()
        {
            IniFile tFilesINI = new IniFile();

            string str = tFilesINI.INIRead("Font", "FontFamily", mSystemInfo.mstrConfigFilePath);
            FontCombox.SelectedIndex = FindFont(str);
            FontCombox.FontFamily = new System.Windows.Media.FontFamily(str);

            str = tFilesINI.INIRead("Font", "FontFamilyEN", mSystemInfo.mstrConfigFilePath);
            FontComboxEN.SelectedIndex = FindFont(str);
            FontComboxEN.FontFamily = new System.Windows.Media.FontFamily(str);

            str = tFilesINI.INIRead("Font", "FontFamilyNumber", mSystemInfo.mstrConfigFilePath);
            FontComboxOrderNumber.SelectedIndex = FindFont(str);
            FontComboxOrderNumber.FontFamily = new System.Windows.Media.FontFamily(str);
        }

        int FindFont(string p_strFontName)
        {
            int nNumber = 0;
            for(int i = 0;i< fontItemList.Count;i++)
            {
                if(fontItemList[i].Name == p_strFontName)
                {
                    nNumber = i;
                    break;
                }
            }

            return nNumber;
        }

        void SaveFont()
        {
            IniFile tFilesINI = new IniFile();

            FontItem tempItem = this.FontCombox.SelectedItem as FontItem;
            tFilesINI.INIWrite("Font", "FontFamily", tempItem.Name, mSystemInfo.mstrConfigFilePath);

            tempItem = this.FontComboxEN.SelectedItem as FontItem;
            tFilesINI.INIWrite("Font", "FontFamilyEN", tempItem.Name, mSystemInfo.mstrConfigFilePath);
            
            tempItem = this.FontComboxOrderNumber.SelectedItem as FontItem;
            tFilesINI.INIWrite("Font", "FontFamilyNumber", tempItem.Name, mSystemInfo.mstrConfigFilePath);
        }
        private void GetLocalItem()
        {
            System.Drawing.FontFamily[] families = null;
            try
            {
                InstalledFontCollection fontCollection = new InstalledFontCollection();
                families = fontCollection.Families;
            }
            catch (Exception ex)
            {
                families = new System.Drawing.FontFamily[0];
                ex.ToString();
            }
            foreach (System.Drawing.FontFamily family in families)
            {
                FontItem fontLocalItem = new FontItem();
                fontLocalItem.Name = family.Name;
                fontItemList.Add(fontLocalItem);
            }


            
        }


        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            Window_About tAbout = new Window_About();
            tAbout.ShowDialog();
        }

        private void buttonPrintTest_Click(object sender, RoutedEventArgs e)
        {
            //WindowTest tTest = new WindowTest();
            //tTest.ShowDialog();

            //打印窗体
            PrintPage printPage = new PrintPage();

            printPage.SetData("5381921979 0", mListText);

            if (printPage.ShowPrintDialog())
            {

                printPage.Show();
                printPage.Print();

                //printPage.Hide();
            }

            printPage.Close();

        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void outsidewrapper_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            //{
            //    previousPoint = e.GetPosition(outside);
            //    isTranslateStart = true;

            //    e.Handled = true;
            //}

        }

        private void outsidewrapper_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            //{
            //    if (isTranslateStart)
            //    {
            //        System.Windows.Point currentPoint = e.GetPosition(outside);  //不能用 inside，必须用outside
            //        Vector v = currentPoint - previousPoint;

            //        TransformGroup tg = canvasOutSide.RenderTransform as TransformGroup;
            //        //TransformGroup tg = Printer.RenderTransform as TransformGroup;

            //        tg.Children.Add(new TranslateTransform(v.X, v.Y));  //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            //                                                            //    inside.RenderTransform = tg;
            //        previousPoint = currentPoint;
            //    }

            //    e.Handled = true;
            //}
        }

        private void outside_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            //{
            //    if (isTranslateStart)
            //    {
            //        isTranslateStart = false;
            //    }

            //    RefreshPosition();
            //    e.Handled = true;
            //}
        }

        private void outside_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //System.Windows.Point currentPoint = e.GetPosition(outside);  //不能用 inside，必须用outside

            //TransformGroup tg = canvasOutSide.RenderTransform as TransformGroup;
            ////TransformGroup tg = Printer.RenderTransform as TransformGroup;

            //double s = ((double)e.Delta) / 1000.0 + 1.0;

            ////centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            //tg.Children.Add(new ScaleTransform(s, s, currentPoint.X, currentPoint.Y));

            //RefreshPosition();
            //e.Handled = true;
        }

        void RefreshPosition()
        {
            if (movingElementCtrl !=null)
            {
                double left = Canvas.GetLeft(movingElementCtrl);
                double top = Canvas.GetTop(movingElementCtrl);
                double width = movingElementCtrl.Width;
                double height = movingElementCtrl.Height;

                RectangleGeometry rg = new RectangleGeometry
                {
                    Rect = new Rect(left, top, width, height)
                };
                originalElement.Data = rg;
                m_CanvasAdorner.SetPosition((int)left, (int)top, (int)width, (int)height);
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!canvas1.IsMouseCaptured)
            {
                startPoint = e.GetPosition(canvas1);
                canvas1.CaptureMouse();

                FrameworkElement c = null;

                if ((e.Source is System.Windows.Controls.Image tImage) && (e.Source != ShowImage))
                {
                    c = tImage as FrameworkElement;
                }

                if (e.Source is System.Windows.Controls.Border tBorder)
                {
                    c = tBorder as FrameworkElement;
                }

                if ((e.Source) is UserControlTextBoxItems textBox)
                {
                    c = textBox as FrameworkElement;
                }

                if (c != null)
                {
                    double left = Canvas.GetLeft(c);
                    double top = Canvas.GetTop(c);
                    double width = c.Width;
                    double height = c.Height;

                    SolidColorBrush borderColor = new SolidColorBrush();

                    originalElement.Stroke = borderColor;
                    RectangleGeometry rg = new RectangleGeometry
                    {
                        Rect = new Rect(left, top, width, height)
                    };

                    isDown = true;
                    originalElement.Data = rg;
                    e.Handled = true;

                    m_CanvasAdorner.SetPosition((int)left, (int)top, (int)width, (int)height);
                    m_CanvasAdorner.SetVisibleState(Visibility.Visible);

                    // SetBindingDataToParameter(m_CanvasAdorner.MPath);
                    //m_CanvasAdorner.mPath = path1;
                    //GetBindingData(path1);
                    //VisibleAreaParameter();

                    if (e.Source != this.border)
                    {
                        m_CanvasAdorner.mFrameworkElement = c;
                        movingElementCtrl = c;
                    }
                }
                else
                {
                    m_CanvasAdorner.SetVisibleState(Visibility.Hidden);
                    m_CanvasAdorner.mPath = null;
                    m_CanvasAdorner.mFrameworkElement = null;
                    movingElementCtrl = null;

                    canvas1.ReleaseMouseCapture();
                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDown)
            {
                DragFinishing(false);
                e.Handled = true;
            }

            mTemplateFundation.GenerateBarCode("5381921979 0  ", ref mTemplate);
            canvas1.ReleaseMouseCapture();

            Debug.Print("ms up!");
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (canvas1.IsMouseCaptured)
            {
                currentPoint = e.GetPosition(canvas1);

                if (isDown)
                {
                    //if (!isDragging
                    //    && (Math.Abs(currentPoint.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance)
                    //    && (Math.Abs(currentPoint.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                    //{
                    //    DragStarting();
                    //}
                    //if (isDragging)
                    //{
                    //    DragMoving();
                    //}

                    if (!isDragging
                        && (Math.Abs(currentPoint.X - startPoint.X) > 2)
                        && (Math.Abs(currentPoint.Y - startPoint.Y) > 2))
                    {
                        DragStarting();
                    }
                    if (isDragging)
                    {
                        DragMoving();
                    }
                }
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
            

        }

        private void DragStarting()
        {
            isDragging = true;
            movingElement = new System.Windows.Shapes.Path();
            movingElement.Data = originalElement.Data;
            movingElement.Fill = selectFillColor;
            movingElement.Stroke = selectBorderColor;
            canvas1.Children.Add(movingElement);
        }

        private void DragMoving()
        {
            currentPoint = Mouse.GetPosition(canvas1);
            TranslateTransform tt = new TranslateTransform();
            tt.X = currentPoint.X - startPoint.X;
            tt.Y = currentPoint.Y - startPoint.Y;
            movingElement.RenderTransform = tt;
        }

        private void DragFinishing(bool cancel)
        {
            Mouse.Capture(null);
            if (isDragging)
            {
                if (!cancel)
                {
                    currentPoint = Mouse.GetPosition(canvas1);
                    TranslateTransform tt0 = new TranslateTransform();
                    TranslateTransform tt = new TranslateTransform();
                    tt.X = currentPoint.X - startPoint.X;
                    tt.Y = currentPoint.Y - startPoint.Y;
                    Geometry geometry =
                    (RectangleGeometry)new RectangleGeometry();
                    string s = originalElement.Data.ToString();
                    if (s == "System.Windows.Media.EllipseGeometry")
                        geometry = (EllipseGeometry)originalElement.Data;
                    else if (s == "System.Windows.Media.RectangleGeometry")
                        geometry = (RectangleGeometry)originalElement.Data;
                    else if (s == "System.Windows.Media.CombinedGeometry")
                        geometry = (CombinedGeometry)originalElement.Data;
                    if (geometry.Transform.ToString() != "Identity")
                    {
                        tt0 = (TranslateTransform)geometry.Transform;
                        tt.X += tt0.X;
                        tt.Y += tt0.Y;
                    }
                    geometry.Transform = tt;

                    //获得模板区域列表索引


                    //canvasCtrl.Children.Remove(originalElement);
                    //originalElement = new System.Windows.Shapes.Path();


                    //originalElement.Fill = fillColor;
                    //originalElement.Fill = arrFillColor[listTemplateArea[nIndex].nType];
                    //originalElement.Stroke = borderColor;
                    originalElement.Data = geometry;
                    //canvasCtrl.Children.Add(originalElement);
                    m_CanvasAdorner.SetPosition((int)originalElement.Data.Bounds.Left
                        , (int)originalElement.Data.Bounds.Top
                        , (int)originalElement.Data.Bounds.Width
                        , (int)originalElement.Data.Bounds.Height);
                    m_CanvasAdorner.SetVisibleState(Visibility.Visible);

                    m_CanvasAdorner.ChangeMpathSize((int)originalElement.Data.Bounds.Left
                        , (int)originalElement.Data.Bounds.Top
                        , (int)originalElement.Data.Bounds.Width
                        , (int)originalElement.Data.Bounds.Height);
                    //m_CanvasAdorner.mPath = originalElement;

                    if (m_CanvasAdorner.mFrameworkElement != null)
                    {
                        //Canvas.SetLeft(m_CanvasAdorner.mFrameworkElement, (int)originalElement.Data.Bounds.Left);
                        //Canvas.SetTop(m_CanvasAdorner.mFrameworkElement, (int)originalElement.Data.Bounds.Top);
                        //m_CanvasAdorner.mFrameworkElement.Width = (int)originalElement.Data.Bounds.Width;
                        //m_CanvasAdorner.mFrameworkElement.Height = (int)originalElement.Data.Bounds.Height;

                        Canvas.SetLeft(movingElementCtrl, (int)originalElement.Data.Bounds.Left);
                        Canvas.SetTop(movingElementCtrl, (int)originalElement.Data.Bounds.Top);
                        movingElementCtrl.Width = (int)originalElement.Data.Bounds.Width;
                        movingElementCtrl.Height = (int)originalElement.Data.Bounds.Height;
                    }

                    
                }
                canvas1.Children.Remove(movingElement);
                movingElement = null;
            }
            isDragging = false;
            isDown = false;
        }

        private void buttonLoadFile_Click(object sender, RoutedEventArgs e)
        {
            
            
            windowExcel.ShowDialog();

        }

        private void buttonPrintConfig_Click(object sender, RoutedEventArgs e)
        {

        }

        private void canvas1_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void buttonSetSize_Click(object sender, RoutedEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");

            if(re.IsMatch(PrintWidth.Text))
            {
                MessageBox.Show("宽度只能输入数字！");
                PrintWidth.Focus();
                PrintWidth.SelectAll();
                return;
            }
            if (re.IsMatch(PrintHeight.Text))
            {
                MessageBox.Show("高度只能输入数字！");
                PrintHeight.Focus();
                PrintHeight.SelectAll();
                return;
            }

            if (re.IsMatch(PrintX.Text))
            {
                MessageBox.Show("X只能输入数字！");
                PrintX.Focus();
                PrintX.SelectAll();
                return;
            }
            if (re.IsMatch(PrintY.Text))
            {
                MessageBox.Show("Y只能输入数字！");
                PrintY.Focus();
                PrintY.SelectAll();
                return;
            }

            var c = canvas1 as FrameworkElement;

            m_dWidth = Math.Round(Convert.ToDouble(PrintWidth.Text),2) * SystemGlobalInfo.const_dScale;
            m_dHeight = Math.Round(Convert.ToDouble(PrintHeight.Text),2) * SystemGlobalInfo.const_dScale;

            c.Width = m_dWidth;
            c.Height = m_dHeight;

            int nLeft = (int)(this.DockPanelOutSide.RenderSize.Width - c.Width) / 2;
            int nTop = (int)(this.DockPanelOutSide.RenderSize.Height - c.Height) / 2;
            Canvas.SetLeft(c, nLeft);
            Canvas.SetTop(c, nTop);

            Canvas.SetLeft(ShowImage, 0);
            Canvas.SetTop(ShowImage, 0);
            ShowImage.Width = m_dWidth;
            ShowImage.Height = m_dHeight;

            ShowImage.Source = bitmap;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //调整每5个一组的那些控件位置
            UserControlTextBoxItems UserControlTextBoxItems = null;
            UserControlTextBoxItems UserControlTextBoxItemsSub = null;
            double dStartY = 0.0;
            for (int i = 5; i < 65; i++)
            {
                if (i % 5 == 0)
                {
                    UserControlTextBoxItems = mTemplate.listText[i];

                    dStartY = Canvas.GetTop(UserControlTextBoxItems);
                }
                else
                {
                    UserControlTextBoxItemsSub = mTemplate.listText[i];
                    Canvas.SetLeft(UserControlTextBoxItemsSub, Canvas.GetLeft(UserControlTextBoxItems));
                    Canvas.SetTop(UserControlTextBoxItemsSub, dStartY);
                    UserControlTextBoxItemsSub.Width = UserControlTextBoxItems.Width;
                    UserControlTextBoxItemsSub.Height = UserControlTextBoxItems.Height;
                }
                dStartY += Convert.ToDouble(GeneralRowWordSpacing.SelectedItem.ToString());
            }

            


            mTemplateFundation.SaveIni(mSystemInfo.mstrConfigFilePath, mTemplate
                , Math.Round(Convert.ToDouble(PrintWidth.Text), 2), Math.Round(Convert.ToDouble(PrintHeight.Text), 2)
                , Math.Round(Convert.ToDouble(PrintX.Text), 2), Math.Round(Convert.ToDouble(PrintY.Text), 2));
            SaveFont();
            SaveConfig();
        }

        private void CheckPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckPrintBarcode.IsChecked)
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Hidden;
            }
        }

        private void CheckPrintOrderNumber_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CheckPrintOrderNumber.IsChecked)
            {
                mTemplate.listText[0].Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.listText[0].Visibility = Visibility.Hidden;
            }
        }

        private void CheckBoldFontOrderNumber_Click(object sender, RoutedEventArgs e)
        {
            int nVal = (bool)CheckBoldFontOrderNumber.IsChecked ? 1 : 0;
            mTemplateFundation.SetNumberFontWeight(ref mTemplate, nVal);
        }

        private void CheckBoldFont_Click(object sender, RoutedEventArgs e)
        {
            int nVal = (bool)CheckBoldFont.IsChecked ? 1 : 0;
            mTemplateFundation.SetGeneralFontWeight(ref mTemplate, nVal);
        }

        //设置常规字体
        private void FontCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((FontCombox.SelectedItem != null) && (FontComboxEN.SelectedItem != null))
            {
                FontItem tempItem = this.FontCombox.SelectedItem as FontItem;
                FontItem tempItemEN = this.FontComboxEN.SelectedItem as FontItem;
                mTemplateFundation.SetGeneralFont(ref mTemplate, tempItem.Name, tempItemEN.Name);
            }
        }
        //设置数字字体
        private void FontComboxOrderNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontItem tempItem = this.FontComboxOrderNumber.SelectedItem as FontItem;
            mTemplateFundation.SetNumberFont(ref mTemplate, tempItem.Name);
        }

        private void FontSizeOrderNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mTemplateFundation.SetNumberFontSize(ref mTemplate, Convert.ToInt32(FontSizeOrderNumber.Text), Convert.ToInt32(FontSizeOrderNumber.Text));
            if (FontSizeOrderNumber.SelectedItem != null)
            {
                mTemplateFundation.SetNumberFontSize(ref mTemplate, Convert.ToInt32(FontSizeOrderNumber.SelectedItem.ToString()), Convert.ToInt32(FontSizeOrderNumber.SelectedItem.ToString()));
            }
        }

        private void FontWordSpacingOrderNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontWordSpacingOrderNumber.SelectedItem != null)
            {
                mTemplateFundation.SetNumberFontWordSpacing(ref mTemplate, Convert.ToInt32(FontWordSpacingOrderNumber.SelectedItem.ToString())); 
            }
        }

        private void FontSizeCH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((FontSizeCH.SelectedItem != null) && (FontSizeNumber.SelectedItem != null))
            {
                int nFontSizeCH = FontSizeCH.SelectedItem.ToString() == "" ? 0 : Convert.ToInt32(FontSizeCH.SelectedItem.ToString());
                int nFontSizeNumber = FontSizeNumber.SelectedItem.ToString() == "" ? 0 : Convert.ToInt32(FontSizeNumber.SelectedItem.ToString());
                mTemplateFundation.SetGeneralFontSize(ref mTemplate, nFontSizeCH, nFontSizeNumber);
            }
        }

        private void FontSizeNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((FontSizeCH.SelectedItem != null) && (FontSizeNumber.SelectedItem != null))
            {
                //int nFontSizeCH = 1;
                //int nFontSizeNumber = 1;
                //mTemplateFundation.SetGeneralFontSize(ref mTemplate, nFontSizeCH, nFontSizeNumber);

                int nFontSizeCH = FontSizeCH.SelectedItem.ToString() == "" ? 1 : Convert.ToInt32(FontSizeCH.SelectedItem.ToString());
                int nFontSizeNumber = FontSizeNumber.SelectedItem.ToString() == "" ? 1 : Convert.ToInt32(FontSizeNumber.SelectedItem.ToString());
                mTemplateFundation.SetGeneralFontSize(ref mTemplate, nFontSizeCH, nFontSizeNumber);
            }
        }

        private void FontWordSpacing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FontWordSpacing.SelectedItem != null)
            {
                mTemplateFundation.SetGeneralFontWordSpacing(ref mTemplate, Convert.ToInt32(FontWordSpacing.SelectedItem.ToString()));
            }
        }

        private void RowWordSpacing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RowWordSpacing.SelectedItem != null)
            {
                mTemplateFundation.SetRowWordSpacing(ref mTemplate, Convert.ToInt32(RowWordSpacing.SelectedItem.ToString()));
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            mSystemInfo.mbExit = true;
            windowExcel.Close();
        }

        private void SetTextBoxAlignment(UserControlTextBoxItems UserControlTextBoxItems)
        {
            UserControlTextBoxItems.mnAlignment = FontAlignment.SelectedIndex;
            UserControlTextBoxItems.SetAlignment();
        }

        private void SetFontAlignment(Object SelectedIndex)
        {
            //票价数字
            SetTextBoxAlignment(mTemplate.listText[66]);
            //基建数字
            SetTextBoxAlignment(mTemplate.listText[68]);
            //燃油数字
            SetTextBoxAlignment(mTemplate.listText[70]);
            //税费数字
            SetTextBoxAlignment(mTemplate.listText[72]);
            //合计数字
            SetTextBoxAlignment(mTemplate.listText[74]);

        }

        private void FontAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetFontAlignment(FontAlignment.SelectedIndex);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
            {
                Debug.Print("Key up keyDown");
            }
            
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(movingElementCtrl != null)
            {
                double left = Canvas.GetLeft(movingElementCtrl);
                double top = Canvas.GetTop(movingElementCtrl);
                double width = movingElementCtrl.Width;
                double height = movingElementCtrl.Height;

                if (e.Key == Key.Up)
                {
                    top -= 1;
                }
                if (e.Key == Key.Down)
                {
                    top += 1;
                }
                if (e.Key == Key.Left)
                {
                    left -= 1;
                }
                if (e.Key == Key.Right)
                {
                    left += 1;
                }

                Canvas.SetLeft(movingElementCtrl, left);
                Canvas.SetTop(movingElementCtrl, top);

                RectangleGeometry rg = new RectangleGeometry
                {
                    Rect = new Rect(left, top, width, height)
                };
                originalElement.Data = rg;
                m_CanvasAdorner.SetPosition((int)left, (int)top, (int)width, (int)height);


            }
            
        }

        private void GeneralRowWordSpacing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //UserControlTextBoxItems UserControlTextBoxItems = p_Template.listText[i];

        }

        private void HiddenTextBoxItems()
        {
            UserControlTextBoxItems UserControlTextBoxItems;
            for (int i = 5; i < 65; i++)
            {
                if (i % 5 != 0)
                {
                    UserControlTextBoxItems = mTemplate.listText[i];
                    UserControlTextBoxItems.Visibility = Visibility.Hidden;
                }
            }

            UserControlTextBoxItems = mTemplate.listText[11];
            UserControlTextBoxItems.Visibility = Visibility.Visible;
            UserControlTextBoxItems = mTemplate.listText[12];
            UserControlTextBoxItems.Visibility = Visibility.Visible;
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        public void RefreshOffset()
        {
            mTemplateFundation.LoadOffset(mSystemInfo.mstrConfigFilePath, ref m_dOffsetX, ref m_dOffsetY);
            PrintX.Text = Math.Round(m_dOffsetX, 2).ToString();
            PrintY.Text = Math.Round(m_dOffsetY, 2).ToString();

            Debug.Print("Call RefreshOffset");
        }
    }

}
