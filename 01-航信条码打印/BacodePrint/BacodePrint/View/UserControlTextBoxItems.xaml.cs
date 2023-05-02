using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
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
        //是否加粗：1-加粗、0-不加粗
        public int mnIsFontWeight { get;set; }

        //对齐方式：0-两端、1-Center、2-Right
        public int mnAlignment { get; set; } = 0;

        string mString = "";

        public UserControlTextBoxItems()
        {
            InitializeComponent();

            mnRowSpacing = 1;
            mnColumnSpacing = 1;
            mnCHFontSize = 12;
            mnENFontSize = 10;
            mnIsFontWeight = 0;

            SetAlignment();
        }

        public ItemsControl GetItem()
        {
            return this.itemCtrl;
        }

        public void SetSpacing()
        {
            for (int i = 0; i < itemCtrl.Items.Count; i++)
            {
                TextBlock block1 = (TextBlock)itemCtrl.Items[i];
                block1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing, 0, 0);
            }
        }

        public void SetSpacing(int p_nRowSpacing,int p_nColumnSpacing)
        {
            mnColumnSpacing = p_nColumnSpacing;
            mnRowSpacing = p_nRowSpacing;

            //SetSpacing();
            SetString();
        }

        public void SetFontWeight(int p_nFontWeight)
        {
            mnIsFontWeight = p_nFontWeight;

            for (int i = 0; i < itemCtrl.Items.Count; i++)
            {
                TextBlock block1 = (TextBlock)itemCtrl.Items[i];
                if (mnIsFontWeight == 1)
                {
                    block1.FontWeight = FontWeights.Bold;
                }
                else
                {
                    block1.FontWeight = FontWeights.Normal;
                }
            }
        }

        public void SetFontSize()
        {
            SetString();
        }

        public void SetFontSize(int p_nCHFontSize,int p_nENFontSize)
        {
            mnCHFontSize = p_nCHFontSize;
            mnENFontSize = p_nENFontSize;

            SetString();
            //SetFontSize();
        }

        public void SetFontFamily()
        {
            itemCtrl.FontFamily = this.FontFamily;
            for (int i = 0; i < itemCtrl.Items.Count; i++)
            {
                TextBlock block1 = (TextBlock)itemCtrl.Items[i];
                block1.FontFamily = itemCtrl.FontFamily;
            }
        }

        public void SetString()
        {
            itemCtrl.Items.Clear();
            itemCtrl.FontSize = mnCHFontSize > mnENFontSize ? mnCHFontSize : mnENFontSize;
            this.FontSize = itemCtrl.FontSize;

            for (int i = 0; i < mString.Length; i++)
            {
                TextBlock block1 = new TextBlock();

                //判断当前字符是否是中文
                if (IsChChar(mString[i]))
                {
                    block1.FontSize = mnCHFontSize;
                }
                else
                {
                    block1.FontSize = mnENFontSize;
                }

                block1.FontFamily = itemCtrl.FontFamily;
                //是否加粗
                if (mnIsFontWeight == 1)
                {
                    block1.FontWeight = FontWeights.Bold;
                }
                else
                {
                    block1.FontWeight = FontWeights.Normal;
                }

                block1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing, 0, 0);
                block1.Text = mString[i].ToString();

                block1.VerticalAlignment = VerticalAlignment.Bottom;

                itemCtrl.Items.Add(block1);
            }
        }
        public void SetString(string p_String)
        {
            mString = p_String;
            SetString();
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

        //设置文本齐方式
        public void SetAlignment()
        {
            var fe = this as FrameworkElement;

            switch (mnAlignment)
            {

                case 0:
                    {
                        var tResource = fe.FindResource("HorizontalAlignmentDefult");
                        itemCtrl.ItemsPanel = (ItemsPanelTemplate)tResource;
                    }
                    break;
                case 1:
                    {
                        var tResource = fe.FindResource("HorizontalAlignmentCenter");
                        itemCtrl.ItemsPanel = (ItemsPanelTemplate)tResource;
                    }
                    break;
                case 2:
                    {
                        var tResource = fe.FindResource("HorizontalAlignmentRight");
                        itemCtrl.ItemsPanel = (ItemsPanelTemplate)tResource;
                    }
                    break;
                default:
                    {
                        var tResource = fe.FindResource("HorizontalAlignmentDefult");
                        itemCtrl.ItemsPanel = (ItemsPanelTemplate)tResource;
                    }
                    break;
            }

        }

    }
}
