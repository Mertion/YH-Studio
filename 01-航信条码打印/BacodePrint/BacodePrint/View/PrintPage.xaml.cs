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
        double nWidth = 0;
        double nHeight = 0;
        double dOffsetX = 0.0;
        double dOffsetY = 0.0;

        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        Template mTemplate = new Template();
        TemplateFundation mTemplateFundation = new TemplateFundation();

        //PrintDialog printDialog = new PrintDialog();
        PrintDialog printDialog = null;

        bool bIsPrinterReady = false;
        public PrintPage()
        {
            InitializeComponent();

            initDilog();

            //获取打印机分辨率
            //int x = (int)printDialog.PrintTicket.PageResolution.X;
            //int y = (int)printDialog.PrintTicket.PageResolution.Y;
            //double dx = printDialog.PrintableAreaWidth;
            //double dy = printDialog.PrintableAreaHeight;    
            
        }

        void initDilog()
        {
            //const double const_dScale = 4.285714285714286;

            IniFile tFilesINI = new IniFile();
            string str = "";
            
            
            mTemplateFundation.LoadTemplateToCanvas(mTemplate, ref this.canvasCtrl);
            mTemplateFundation.LoadTemplateFromIni(mSystemInfo.mstrConfigFilePath, ref mTemplate, ref this.canvasCtrl
                , ref nWidth, ref nHeight, ref dOffsetX, ref dOffsetY);

            this.canvasCtrl.Width = Math.Round(nWidth, 2) * SystemGlobalInfo.const_dScale;
            this.canvasCtrl.Height = Math.Round(nHeight, 2) * SystemGlobalInfo.const_dScale;
            //GridPrint.Margin = new Thickness(500, 300, 0, 0);
            this.Width = canvasCtrl.Width + 100;
            this.Height = canvasCtrl.Height + 100;

            str = tFilesINI.INIRead("Config", "PrintNumber", mSystemInfo.mstrConfigFilePath);

            if (str != "0")
            {
                mTemplate.listText[0].Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.listText[0].Visibility = Visibility.Hidden;
            }

            str = tFilesINI.INIRead("Config", "PrintBarcode", mSystemInfo.mstrConfigFilePath);

            if (str != "0")
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Visible;
            }
            else
            {
                mTemplate.mImageBarcode.Visibility = Visibility.Hidden;
            }
        }

        public void Print()
        {
            if (bIsPrinterReady)
            {
                //printDialog.PrintVisual(canvasCtrl, "Print");

                printDialog.PrintVisual(GridPrint, "Print");
            }
            else
            {
                MessageBox.Show("请选择打印机！");
            }
        }

        public void SetData(string p_strBarcode, List<string> p_ListText)
        {
            if (p_ListText != null)
            {
                mTemplateFundation.SetTemplateData(p_strBarcode, p_ListText, ref mTemplate);
            }
        }

        public void SetPageSize()
        {
            if(printDialog!=null)
            {
                this.canvasCtrl.Width = Math.Round(nWidth, 2) * SystemGlobalInfo.const_dScale;
                this.canvasCtrl.Height = Math.Round(nHeight, 2) * SystemGlobalInfo.const_dScale;
                this.canvasCtrl.Margin
                    = new Thickness(Math.Round(dOffsetX, 2) * SystemGlobalInfo.const_dScale
                    , Math.Round(dOffsetY, 2) * SystemGlobalInfo.const_dScale
                    , 0, 0);

                GridPrint.Width = printDialog.PrintableAreaWidth;
                GridPrint.Height = printDialog.PrintableAreaHeight;
                this.Width = GridPrint.Width + 100;
                this.Height = GridPrint.Height + 100;
            }
        }
        public void SetPrintDialog(PrintDialog p_printDialog)
        {
            bIsPrinterReady = true;

            printDialog = p_printDialog;

            SetPageSize();
        }
        public bool ShowPrintDialog()
        {
            if (printDialog.ShowDialog() == true)
            {
                //横向-Landscape;、纵向-Portrait
                //printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                
                bIsPrinterReady = true;

                //this.canvasCtrl.Width = printDialog.PrintableAreaWidth;
                //this.canvasCtrl.Height = printDialog.PrintableAreaHeight;
                //GridPrint.Width = this.canvasCtrl.Width;
                //GridPrint.Height = this.canvasCtrl.Height;
                //this.Width = canvasCtrl.Width + 100;
                //this.Height = canvasCtrl.Height + 100;

                SetPageSize();
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
