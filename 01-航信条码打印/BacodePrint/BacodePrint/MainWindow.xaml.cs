using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
    }


}
