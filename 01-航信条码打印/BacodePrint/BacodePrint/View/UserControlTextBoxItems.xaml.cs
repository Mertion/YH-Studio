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
using static System.Net.Mime.MediaTypeNames;

namespace BacodePrint
{
    /// <summary>
    /// UserControlTextBoxItems.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlTextBoxItems : UserControl
    {
        public int mnRowSpacing { get; set;}
        public int mnColumnSpacing { get; set; }
        //中文字体大小
        public int mnCHFontSize { get; set; }
        //英文字体大小
        public int mnENFontSize { get; set; }
        public UserControlTextBoxItems()
        {
            InitializeComponent();

            mnRowSpacing = 1;
            mnColumnSpacing = 1;
            mnCHFontSize = 12;
            mnENFontSize = 10;
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

        public void SetFontSize(int p_nCHFontSize,int p_nENFontSize)
        {
            mnCHFontSize = p_nCHFontSize;
            mnENFontSize = p_nENFontSize;

            for (int i = 0;i < itemCtrl.Items.Count;i++)
            {
                Border border1 = (Border)itemCtrl.Items[i];
                TextBlock block1 = (TextBlock)border1.Child;

                char c= block1.Text[0];
                if (IsChChar(c))
                {
                    block1.FontSize = mnCHFontSize;
                }
                else
                {
                    block1.FontSize = mnENFontSize;
                }
            }
        }

        public void SetString(string p_String)
        {
            itemCtrl.Items.Clear();

            for (int i = 0; i < p_String.Length; i++)
            {
                //Border border1 = new Border();
                //border1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing, 0, 0);
                //ContentPresenter contentPresenter1 = new ContentPresenter();
                //contentPresenter1.Content = p_String[i];
                //border1.Child = contentPresenter1;
                //border1.VerticalAlignment = VerticalAlignment.Bottom;
                //itemCtrl.Items.Add(border1);

                TextBlock block1 = new TextBlock();

                block1.Text = p_String[i].ToString();
                if (IsChChar(p_String[i]))
                {
                    block1.FontSize = mnCHFontSize;
                }
                else
                {
                    block1.FontSize = mnENFontSize;
                }

                block1.FontFamily = itemCtrl.FontFamily;
                block1.VerticalAlignment = VerticalAlignment.Bottom;
                itemCtrl.Items.Add(block1);
            }
        }

        bool IsChChar(char p_char)
        {
            if(p_char >= 0x4e00 && p_char <= 0x9fbb)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}
