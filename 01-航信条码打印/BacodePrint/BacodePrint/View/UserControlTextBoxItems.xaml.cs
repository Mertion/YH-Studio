﻿using System;
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
        //是否加粗：1-加粗、0-不加粗
        public int mnIsFontWeight { get;set; }

        string mString = "";

        public UserControlTextBoxItems()
        {
            InitializeComponent();

            mnRowSpacing = 1;
            mnColumnSpacing = 1;
            mnCHFontSize = 12;
            mnENFontSize = 10;
            mnIsFontWeight = 0;
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

            SetSpacing();
        }

        public void SetFontWeight(int p_nFontWeight)
        {
            mnIsFontWeight=p_nFontWeight;

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
            //for (int i = 0; i < itemCtrl.Items.Count; i++)
            //{
            //    //Border border1 = (Border)itemCtrl.Items[i];
            //    TextBlock block1 = (TextBlock)itemCtrl.Items[i];

            //    char c = block1.Text[0];
            //    if (IsChChar(c))
            //    {
            //        block1.FontSize = mnCHFontSize;
            //    }
            //    else
            //    {
            //        block1.FontSize = mnENFontSize;
            //    }
            //}
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
            for (int i = 0; i < itemCtrl.Items.Count; i++)
            {
                TextBlock block1 = (TextBlock)itemCtrl.Items[i];
                block1.FontFamily = itemCtrl.FontFamily;
            }
        }

        public void SetString()
        {
            itemCtrl.Items.Clear();

            for (int i = 0; i < mString.Length; i++)
            {
                //Border border1 = new Border();
                //border1.Margin = new Thickness(mnColumnSpacing, mnRowSpacing, 0, 0);
                //ContentPresenter contentPresenter1 = new ContentPresenter();
                //contentPresenter1.Content = p_String[i];
                //border1.Child = contentPresenter1;
                //border1.VerticalAlignment = VerticalAlignment.Bottom;
                //itemCtrl.Items.Add(border1);

                TextBlock block1 = new TextBlock();

                block1.Text = mString[i].ToString();

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
       
    }
}
