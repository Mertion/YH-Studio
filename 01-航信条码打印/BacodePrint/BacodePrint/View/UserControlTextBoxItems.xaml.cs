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
        public int mnRowSpacing { get; set;}
        public int mnColumnSpacing { get; set; }
        public UserControlTextBoxItems()
        {
            InitializeComponent();

            mnRowSpacing = 1;
            mnColumnSpacing = 1;
        }

        public ItemsControl GetItem()
        {
            return this.itemCtrl;
        }

        public void SetSpacing(int p_nRowSpacing,int p_nColumnSpacing)
        {
            mnColumnSpacing = p_nColumnSpacing;
            mnRowSpacing = p_nRowSpacing;

            for (int i = 0; i < itemCtrl.Items.Count; i++) 
            {
                Border border1 = (Border)itemCtrl.Items[i];
                border1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing, 0, 0);
            }
        }

        public void SetString(string p_String)
        {
            itemCtrl.Items.Clear();

            for (int i = 0; i < p_String.Length; i++)
            {
                Border border1 = new Border();
                border1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing,0,0);
                ContentPresenter contentPresenter1 = new ContentPresenter();
                contentPresenter1.Content = p_String[i];
                border1.Child = contentPresenter1;

                itemCtrl.Items.Add(border1);

            }
        }
    }
}
