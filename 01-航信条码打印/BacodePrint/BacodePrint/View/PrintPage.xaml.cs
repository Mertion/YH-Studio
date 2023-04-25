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
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        Template mTemplate = new Template();
        TemplateFundation mTemplateFundation = new TemplateFundation();

        public PrintPage()
        {
            InitializeComponent();
        }

        public void PrintWindows()
        {
            PrintDialog printDialog = new PrintDialog();

            printDialog.PrintVisual(GridPrint, "Test Print");
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
                
                
            }

           
        }

        public UserControlPrint GetPrinter()
        {
            //return this.Printer;
            return null;
        }
    }
}
