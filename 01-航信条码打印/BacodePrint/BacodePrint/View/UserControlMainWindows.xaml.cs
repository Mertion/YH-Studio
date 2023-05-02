using BacodePrint.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
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
using System.Windows.Shell;

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
        //页面尺寸
        const double const_dScale = 4.285714285714286;
        int mWidth = 1800;
        int mHeight = 900;

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

        public UserControlMainWindows()
        {
            InitializeComponent();
            
            //更新字体列表
            GetLocalItem();
            this.FontCombox.ItemsSource = fontItemList;
            FontComboxOrderNumber.ItemsSource = fontItemList;
            ReadFont();

            //初始化字号列表和其它配置信息
            {
                for (int i = 0;i<30;i++)
                {
                    FontSizeCH.Items.Add(i.ToString());
                    FontSizeNumber.Items.Add(i.ToString());
                    FontWordSpacing.Items.Add(i.ToString());
                    RowWordSpacing.Items.Add(i.ToString());
                    FontSizeOrderNumber.Items.Add(i.ToString());
                    FontWordSpacingOrderNumber.Items.Add((i+1).ToString());
                }

                FontAlignment.Items.Add("左对齐");
                FontAlignment.Items.Add("右对齐");
                FontAlignment.SelectedIndex = 0;
                ReadConfig();
            }
            int nWidth = 0;
            int nHeight= 0;

            int nLeft = 0;
            int nTop = 0;

            string str = "";
            mListText.Clear();

            str = "印刷序号";
            mListText.Add(str);
            str = "姓名";
            mListText.Add(str);
            str = "证件号";
            mListText.Add(str);
            str = "签注";
            mListText.Add(str);
            str = "PNR";
            mListText.Add(str);

            str = "起飞航站楼1|";
            mListText.Add(str);
            str = "起飞航站楼2|";
            mListText.Add(str);
            str = "起飞航站楼3|";
            mListText.Add(str);
            str = "起飞航站楼4|";
            mListText.Add(str);
            str = "起飞航站楼5|";

            mListText.Add(str);
            str = "起飞地_自FROM|";
            mListText.Add(str);
            str = "到达地_至1|";
            mListText.Add(str);
            str = "至2|";
            mListText.Add(str);
            str = "至3|";
            mListText.Add(str);
            str = "至4|";
            
            mListText.Add(str);
            str = "到达航站楼1|";
            mListText.Add(str);
            str = "到达航站楼2|";
            mListText.Add(str);
            str = "到达航站楼3|";
            mListText.Add(str);
            str = "到达航站楼4|";
            mListText.Add(str);
            str = "到达航站楼5|";
            mListText.Add(str);

            str = "承运人1|";
            mListText.Add(str);
            str = "承运人2|";
            mListText.Add(str);
            str = "承运人3|";
            mListText.Add(str);
            str = "承运人4|";
            mListText.Add(str);
            str = "承运人5|";
            mListText.Add(str);

            str = "航班号1|";
            mListText.Add(str);
            str = "航班号2|";
            mListText.Add(str);
            str = "航班号3|";
            mListText.Add(str);
            str = "航班号4|";
            mListText.Add(str);
            str = "航班号5|";
            mListText.Add(str);

            str = "座位等级1|";
            mListText.Add(str);
            str = "等级2|";
            mListText.Add(str);
            str = "等级3|";
            mListText.Add(str);
            str = "等级4|";
            mListText.Add(str);
            str = "等级5";
            mListText.Add(str);

            str = "日期1|";
            mListText.Add(str);
            str = "日期2|";
            mListText.Add(str);
            str = "日期3|";
            mListText.Add(str);
            str = "日期4|";
            mListText.Add(str);
            str = "日期5";
            mListText.Add(str);

            str = "时间1|";
            mListText.Add(str);
            str = "时间2|";
            mListText.Add(str);
            str = "时间3|";
            mListText.Add(str);
            str = "时间4|";
            mListText.Add(str);
            str = "时间5";
            mListText.Add(str);

            str = "客票级别1|";
            mListText.Add(str);
            str = "客票级别2|";
            mListText.Add(str);
            str = "客票级别3|";
            mListText.Add(str);
            str = "客票级别4|";
            mListText.Add(str);
            str = "客票级别5|";
            mListText.Add(str);

            str = "客票生效日期1|";
            mListText.Add(str);
            str = "生效2|";
            mListText.Add(str);
            str = "生效3|";
            mListText.Add(str);
            str = "生效4|";
            mListText.Add(str);
            str = "生效5|";
            mListText.Add(str);

            str = "客票截止日期1|";
            mListText.Add(str);
            str = "客票截止日期2|";
            mListText.Add(str);
            str = "客票截止日期3|";
            mListText.Add(str);
            str = "客票截止日期4|";
            mListText.Add(str);
            str = "客票截止日期5|";
            mListText.Add(str);

            str = "免费行李1|";
            mListText.Add(str);
            str = "行李2|";
            mListText.Add(str);
            str = "行李3|";
            mListText.Add(str);
            str = "行李4|";
            mListText.Add(str);
            str = "行李5|";
            mListText.Add(str);

            str = "票价字母";
            mListText.Add(str);
            str = "票价数字";
            mListText.Add(str);
            str = "基建字母";
            mListText.Add(str);
            str = "基建数字";
            mListText.Add(str);
            str = "燃油字母";
            mListText.Add(str);
            str = "燃油数字";
            mListText.Add(str);
            str = "税费字母";
            mListText.Add(str);
            str = "税费数字";
            mListText.Add(str);
            str = "合计字母";
            mListText.Add(str);
            str = "合计数字";
            mListText.Add(str);
            str = "票号";
            mListText.Add(str);
            str = "验证码";
            mListText.Add(str);
            str = "提示信息";
            mListText.Add(str);
            str = "保险费";
            mListText.Add(str);
            str = "销售单位代码1|";
            mListText.Add(str);
            str = "销售单位代码2|";
            mListText.Add(str);

            str = "填开单位";
            mListText.Add(str);
            str = "填开日期";
            mListText.Add(str);

            mTemplateFundation.LoadTemplateToCanvas(mTemplate, ref this.canvas1);
            mTemplateFundation.LoadTemplateFromIni(mSystemInfo.mstrConfigFilePath, ref mTemplate, ref this.canvas1, ref nWidth, ref nHeight);
            mTemplateFundation.SetTemplateData("5381921979 0", mListText, ref mTemplate);
            mTemplateFundation.SetBorderThickness(ref mTemplate, 2);

            mWidth = nWidth;
            mHeight = nHeight;
            this.canvas1.Width = mWidth;
            this.canvas1.Height = mHeight;
            PrintWidth.Text = ((int)(mWidth / const_dScale)).ToString();
            PrintHeight.Text = ((int)(mHeight / const_dScale)).ToString();

            nLeft = ((int)this.DockPanelOutSide.Width - nWidth)/2;
            nTop = ((int)this.DockPanelOutSide.Height - nHeight)/2;
            Canvas.SetLeft(this.canvas1, nLeft);
            Canvas.SetTop(this.canvas1, nTop);
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

                if (e.Source is System.Windows.Controls.Image tImage)
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

            canvas1.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (canvas1.IsMouseCaptured)
            {
                currentPoint = e.GetPosition(canvas1);

                if (isDown)
                {
                    if (!isDragging
                        && (Math.Abs(currentPoint.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance)
                        && (Math.Abs(currentPoint.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
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
            //movingElement.Fill = selectFillColor;
            //movingElement.Stroke = selectBorderColor;
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

                    mTemplateFundation.GenerateBarCode("5381921979 0", ref mTemplate);
                }
                canvas1.Children.Remove(movingElement);
                movingElement = null;
            }
            isDragging = false;
            isDown = false;
        }

        private void buttonLoadFile_Click(object sender, RoutedEventArgs e)
        {
            WindowExcel windowExcel = new WindowExcel();
            windowExcel.ReadDataFromExcel();
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
            var c = canvas1 as FrameworkElement;

            mWidth = (int)( Convert.ToInt32(PrintWidth.Text) * const_dScale);
            mHeight = (int)(Convert.ToInt32(PrintHeight.Text) * const_dScale);

            c.Width = Convert.ToInt32(mWidth);
            c.Height = Convert.ToInt32(mHeight);

            int nLeft = (int)(this.DockPanelOutSide.RenderSize.Width - c.Width) / 2;
            int nTop = (int)(this.DockPanelOutSide.RenderSize.Height - c.Height) / 2;
            Canvas.SetLeft(c, nLeft);
            Canvas.SetTop(c, nTop);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            mTemplateFundation.SaveIni(mSystemInfo.mstrConfigFilePath, mTemplate, (int)canvas1.Width, (int)canvas1.Height);
            SaveFont();
            SaveConfig();
        }

        //设置常规字体
        private void FontCombox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mTemplateFundation.SetGeneralFont(ref mTemplate, FontCombox.Text);
        }
        //设置序号字体
        private void FontComboxOrderNumber_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mTemplateFundation.SetGeneralFont(ref mTemplate, FontComboxOrderNumber.Text);
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
            FontItem tempItem = this.FontCombox.SelectedItem as FontItem;
            mTemplateFundation.SetGeneralFont(ref mTemplate, tempItem.Name);
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
    }

}
