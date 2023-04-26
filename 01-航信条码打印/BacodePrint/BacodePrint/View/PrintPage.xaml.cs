using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        Template mTemplate = new Template();
        TemplateFundation mTemplateFundation = new TemplateFundation();

        PrintDialog printDialog = new PrintDialog();
        bool bIsPrinterReady = false;
        public PrintPage()
        {
            InitializeComponent();
        }

        public void PrintWindows()
        {
            if (bIsPrinterReady)
            {
                printDialog.PrintVisual(canvasCtrl, "Print");
            }
            else
            {
                MessageBox.Show("请选择打印机！");
            }
        }

        public void Print(string p_strBarcode, List<string> p_ListText)
        {
            if (p_ListText != null)
            {
                int nWidth = 0;
                int nHeight = 0;
                mTemplateFundation.LoadTemplateToCanvas(mTemplate, ref this.canvasCtrl);
                mTemplateFundation.LoadTemplateFromIni(mSystemInfo.mstrConfigFilePath, ref mTemplate, ref this.canvasCtrl, ref nWidth, ref nHeight);
                mTemplateFundation.SetTemplateData(p_strBarcode, p_ListText, ref mTemplate);

                this.canvasCtrl.Width = nWidth;
                this.canvasCtrl.Height = nHeight;
                GridPrint.Margin = new Thickness(500, 300, 0, 0);
                this.Width = nWidth+100;
                this.Height = nHeight+100;
            }
        }

        public bool ShowPrintDialog()
        {
            if (printDialog.ShowDialog() == true)
            {
                //横向-Landscape;、纵向-Portrait
                //printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                
                bIsPrinterReady = true;
            }

            return bIsPrinterReady;
        }

        public UserControlPrint GetPrinter()
        {
            //return this.Printer;
            return null;
        }
    }
}
