using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace BacodePrint.View
{
    /// <summary>
    /// WindowPrintf.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPrintf : Window
    {
        public WindowPrintf()
        {
            InitializeComponent();
            Preview();
        }

        private void Preview()
        {
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Print.xps";
            XpsDocument doc = new XpsDocument(filename, System.IO.FileAccess.Read);
            docviewer.Document = doc.GetFixedDocumentSequence();
            doc.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            //退出预览窗体时删除保存的XPS打印文件
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Print.xps");
        }
    }
}
