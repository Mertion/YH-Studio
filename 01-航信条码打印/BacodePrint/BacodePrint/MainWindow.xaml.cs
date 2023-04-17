using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace BacodePrint
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {

        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;

        System.Windows.Point previousPoint;
        bool isTranslateStart = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoadFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPrintConfig_Click(object sender, RoutedEventArgs e)
        {

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
            Printer.SetPrintBarcode("5381921979 0");

            PrintPage tPrintPage = new PrintPage();
            tPrintPage.Width = 1131;
            tPrintPage.Height = 479;
            //tPrintPage.SetPrintBarcode("5381921979 0");

            string str = "5381921979 0" ;
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

                    TransformGroup tg = canvas1.RenderTransform as TransformGroup;
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

            TransformGroup tg = canvas1.RenderTransform as TransformGroup;
            //TransformGroup tg = Printer.RenderTransform as TransformGroup;

            double s = ((double)e.Delta) / 1000.0 + 1.0;

            //centerX和centerY用外部包装元素的坐标，不能用内部被变换的Canvas元素的坐标
            tg.Children.Add(new ScaleTransform(s, s, currentPoint.X, currentPoint.Y));
            e.Handled = true;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void canvas1_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }


}
