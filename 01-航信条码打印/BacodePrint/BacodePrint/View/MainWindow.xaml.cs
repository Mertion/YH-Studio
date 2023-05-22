using BacodePrint.Fundation;
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

        //bool isTranslateStart = false;

        public MainWindow()
        {
            InitializeComponent();

            int nRet = Authentication.MachineCodeAuthenticationDLL();

            if(nRet != 0)
            {
                MessageBox.Show("机器码验证不正确，请联系YH工作室！");

                Close();
            }

            nRet = Authentication.CheckCount();
            if (nRet != 0)
            {
                MessageBox.Show("超过试用次数，请联系YH工作室！");

                Close();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UserControlMain.RefreshOffset();
        }
    }


}
