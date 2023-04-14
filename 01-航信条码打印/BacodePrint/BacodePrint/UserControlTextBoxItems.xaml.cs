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
    /// UserControlTextBoxItems.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlTextBoxItems : UserControl
    {
        public UserControlTextBoxItems()
        {
            InitializeComponent();
        }

        public ItemsControl GetItem()
        {
            return this.itemCtrl;
        }

        public void SetString(string p_String)
        {
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            //textBlockFactoryB.SetBinding(TextBlock.TextProperty, new Binding("ValueB"));
            contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding());
            var border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.MarginProperty, new Thickness(10));
            border.AppendChild(contentPresenter);

            var dataTemplate = new DataTemplate
            {
                VisualTree = border
            };

            itemCtrl.Items.Clear();

            for (int i = 0; i < p_String.Length; i++)
            {
                Border border1 = new Border();
                border1.Margin = new Thickness(10);
                ContentPresenter contentPresenter1 = new ContentPresenter();
                contentPresenter1.Content = p_String[i];
                border1.Child = contentPresenter1;

                itemCtrl.Items.Add(border1);

            }
        }
    }
}
