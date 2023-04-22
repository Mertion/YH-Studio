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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BacodePrint
{
    /// <summary>
    /// UserControlPrint.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlPrint : UserControl
    {
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;

        List<UserControlTextBoxItems> listText = new List<UserControlTextBoxItems>();
        const int nListMaxCount = 34;

        private System.Windows.Point startPoint = new System.Windows.Point();
        //动态调整大小
        private CanvasAdorner m_CanvasAdorner = null;
        System.Windows.Point currentPoint = new System.Windows.Point();
        private bool isDown = false;
        private System.Windows.Shapes.Path originalElement = new System.Windows.Shapes.Path();
        private System.Windows.Shapes.Path movingElement = new System.Windows.Shapes.Path();

        private bool isDragging = false;
        public UserControlPrint()
        {
            InitializeComponent();

            Init();
        }

        //初始化
        private void Init()
        {
            
        }

       

        public void SetPrintBarcode(string strCode)
        {
            BitmapImage tmpImage = ClassBarCode.GenerateBarCodeBitmapImage(strCode);
            this.BarcodeImage.Source = tmpImage;

            Test.itemCtrl.FontSize = 10;
            Test.itemCtrl.FontFamily = new FontFamily("STSong");
            Test.SetString("fdsf小贩之歌刘功臣!!CXVf,.;'po");
            Test.SetSpacing(10, 2);

            AddCtrlList();
        }

        public void AddCtrlList()
        {
            //int nLeft = 30;
            //int nTop = 30;

            //for (int i = 0; i < nListMaxCount; i++)
            //{
            //    nLeft += 20;
            //    nTop += 20;
            //    UserControlTextBoxItems UserControlTextBoxItems = new UserControlTextBoxItems();
            //    Brush brush = new SolidColorBrush(Color.FromArgb(0xff, 0x7f, 0x30, 0x78));
            //    UserControlTextBoxItems.Background = brush;
            //    var c = UserControlTextBoxItems as FrameworkElement;
            //    Canvas.SetLeft(c, nLeft);
            //    Canvas.SetTop(c, nTop);
            //    c.Width = 200;
            //    c.Height = 50;

            //    UserControlTextBoxItems.Visibility = Visibility.Visible;
            //    listText.Add(UserControlTextBoxItems);
            //    canvasCtrl.Children.Add(c);
            //}

          

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        

    }
}
