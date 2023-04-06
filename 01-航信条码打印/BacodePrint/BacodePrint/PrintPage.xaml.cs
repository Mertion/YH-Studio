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
using System.Windows.Shapes;

namespace BacodePrint
{
    /// <summary>
    /// PrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPage : Window
    {
        public PrintPage()
        {
            InitializeComponent();
        }

        public void SetPrintBarcode(string strCode)
        {
            BitmapImage tmpImage = ClassBarCode.GenerateBarCodeBitmapImage(strCode);
            this.BarcodeImage.Source = tmpImage;
           
        }

        public void PrintWindows()
        {
            PrintDialog printDialog = new PrintDialog();

            printDialog.PrintVisual(Print, "Test Print");
        }
    }
}
