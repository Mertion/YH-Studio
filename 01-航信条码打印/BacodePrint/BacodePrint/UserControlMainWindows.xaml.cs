using System;
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
    /// UserControlMainWindows.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMainWindows : UserControl
    {
        System.Windows.Point previousPoint;
        bool isTranslateStart = false;

        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        IniFile mFilesINI = new IniFile();

        List<UserControlTextBoxItems> listText = new List<UserControlTextBoxItems>();
        const int nListMaxCount = 34;

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
            if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                previousPoint = e.GetPosition(outside);
                isTranslateStart = true;

                e.Handled = true;
            }

        }

        private void outsidewrapper_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (isTranslateStart)
                {
                    System.Windows.Point currentPoint = e.GetPosition(outside);  //不能用 inside，必须用outside
                    Vector v = currentPoint - previousPoint;

                    TransformGroup tg = canvasOutSide.RenderTransform as TransformGroup;
                    //TransformGroup tg = Printer.RenderTransform as TransformGroup;

                    tg.Children.Add(new TranslateTransform(v.X, v.Y));  //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
                                                                        //    inside.RenderTransform = tg;
                    previousPoint = currentPoint;
                }

                e.Handled = true;
            }
        }

        private void outside_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                if (isTranslateStart)
                {
                    isTranslateStart = false;
                }

                e.Handled = true;
            }
        }

        private void outside_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point currentPoint = e.GetPosition(outside);  //不能用 inside，必须用outside

            TransformGroup tg = canvasOutSide.RenderTransform as TransformGroup;
            //TransformGroup tg = Printer.RenderTransform as TransformGroup;

            double s = ((double)e.Delta) / 1000.0 + 1.0;

            //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            tg.Children.Add(new ScaleTransform(s, s, currentPoint.X, currentPoint.Y));
            e.Handled = true;
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
    }
}
