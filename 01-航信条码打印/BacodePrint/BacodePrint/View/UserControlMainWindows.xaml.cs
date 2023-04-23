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
        //字体列表
        private ObservableCollection<FontItem> fontItemList = new ObservableCollection<FontItem>();

        Template tTemplate = new Template();
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        TemplateFundation mTemplateFundation = new TemplateFundation();

        List<string> mListText = new List<string>();

        System.Windows.Point previousPoint;
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
            this.FontCombox.SelectedIndex = 0;

            FontComboxOrderNumber.ItemsSource = fontItemList;
            FontComboxOrderNumber.SelectedIndex = 0;

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
            str = "起飞地_自FROM|到达地_至1|至2|";
            mListText.Add(str);
            str = "到达航站楼1|";
            mListText.Add(str);
            str = "承运人1|";
            mListText.Add(str);
            str = "航班号1|航班号2|航班";
            mListText.Add(str);
            str = "座位等级1|等级2|等级3|等级4|等级5";
            mListText.Add(str);
            str = "日期1|日期2|日期3|日期4|日期5";
            mListText.Add(str);
            str = "时间1|时间2|时间3|时间4|时间5";
            mListText.Add(str);
            str = "客票级别1|级别2|";
            mListText.Add(str);
            str = "客票生效日期1|生效2|";
            mListText.Add(str);
            str = "客票截止日期1|截止2|";
            mListText.Add(str);
            str = "免费行李1|行李2|行李3|行李4|行李5";
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
            str = "销售单位代码1|代码2";
            mListText.Add(str);
            str = "填开单位";
            mListText.Add(str);
            str = "填开日期";
            mListText.Add(str);

            mTemplateFundation.LoadTemplateToCanvas(tTemplate, ref this.canvas1);
            mTemplateFundation.LoadTemplateFromIni(mSystemInfo.mstrConfigFilePath, ref tTemplate, ref this.canvas1, ref nWidth, ref nHeight);
            mTemplateFundation.SetTemplateData("5381921979 0", mListText, ref tTemplate);
            mTemplateFundation.SetBorderThickness(ref tTemplate, 2);
            this.canvas1.Width = nWidth;
            this.canvas1.Height = nHeight;
            PrintWidth.Text = nWidth.ToString();
            PrintHeight.Text = nHeight.ToString();

            nLeft = ((int)this.DockPanelOutSide.Width - nWidth)/2;
            nTop = ((int)this.DockPanelOutSide.Height - nHeight)/2;
            Canvas.SetLeft(this.canvas1, nLeft);
            Canvas.SetTop(this.canvas1, nTop);
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
            WindowTest tTest = new WindowTest();
            tTest.ShowDialog();

        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            //Printer.SetPrintBarcode("5381921979 0");

            PrintPage tPrintPage = new PrintPage();
            tPrintPage.Width = 1131;
            tPrintPage.Height = 479;
            //tPrintPage.SetPrintBarcode("5381921979 0");

            string str = "5381921979 0";
            UserControlPrint userControlPrint = tPrintPage.GetPrinter();
            //tPrintPage.ShowDialog();
            userControlPrint.SetPrintBarcode(str);
            tPrintPage.PrintWindows();

            tPrintPage.Close();
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

                    mTemplateFundation.GenerateBarCode("5381921979 0", ref tTemplate);
                }
                canvas1.Children.Remove(movingElement);
                movingElement = null;
            }
            isDragging = false;
            isDown = false;
        }

        private void buttonLoadFile_Click(object sender, RoutedEventArgs e)
        {

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
           
            
            c.Width = Convert.ToInt32(PrintWidth.Text);
            c.Height = Convert.ToInt32(PrintHeight.Text);

            int nLeft = (int)(this.DockPanelOutSide.RenderSize.Width - c.Width) / 2;
            int nTop = (int)(this.DockPanelOutSide.RenderSize.Height - c.Height) / 2;
            Canvas.SetLeft(c, nLeft);
            Canvas.SetTop(c, nTop);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            mTemplateFundation.SaveIni(mSystemInfo.mstrConfigFilePath, tTemplate, (int)canvas1.Width, (int)canvas1.Height);
        }
    }
}
