using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
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
    /// 字体结构体
    /// </summary>
    public class FontItem
    {
        /// <summary>
        /// 字体名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// WindowFontList.xaml 的交互逻辑
    /// </summary>
    public partial class WindowFontList : Window
    {
        public FontItem mFontItem { get; set; }
        

        //public WindowFontList()
        //{
        //    InitializeComponent();
        //}

        private ObservableCollection<FontItem> fontItemList = new ObservableCollection<FontItem>();
       //private ObservableCollection<FontItem> fontItemList1 = new ObservableCollection<FontItem>();
        private ObservableCollection<FontItem> fontItemList1;
        public WindowFontList()
        {
            InitializeComponent();
            GetLocalItem();
            this.fontList.ItemsSource = fontItemList;
            this.fontList.SelectedIndex = 0;

            
            this.fontList1.ItemsSource = fontItemList1;
            this.fontList1.SelectedIndex = 0;

            mFontItem = null;
        }

        /// <summary>
        /// 获取本地字体列表
        /// </summary>
        private void GetLocalItem()
        {
            System.Drawing.FontFamily[] families = null;
            try
            {
                InstalledFontCollection fontCollection = new InstalledFontCollection();
                families = fontCollection.Families;
            }
            catch (Exception ex)
            {
                families = new System.Drawing.FontFamily[0];
                ex.ToString();
            }
            foreach (System.Drawing.FontFamily family in families)
            {
                FontItem fontLocalItem = new FontItem();
                fontLocalItem.Name = family.Name;
                fontItemList.Add(fontLocalItem);
            }


            ObservableCollection<FontItem> fontItemListOrder = new ObservableCollection<FontItem>();
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                //fontList1.Items.Add(fontFamily.Source);
                FontItem fontLocalItem = new FontItem();
                fontLocalItem.Name = fontFamily.Source;
                fontItemListOrder.Add(fontLocalItem);
            }

            //fontItemList1 = new ObservableCollection<FontItem>(fontItemListOrder.OrderByDescending(item => item.Name));
            fontItemList1 = new ObservableCollection<FontItem>(fontItemListOrder.OrderBy(item => item.Name));

            fontCount1.Text = fontItemList.Count.ToString();
            fontCount2.Text = fontItemList1.Count.ToString();
        }

        /// <summary>
        /// 选项改变同时改变展示字体
        /// </summary>
        private void FontList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontItem tempItem = this.fontList.SelectedItem as FontItem;
            if (tempItem != null && tempItem.Name != "")
            {
                this.DisplayFontCh.FontFamily = new FontFamily(tempItem.Name);
                this.DisplayFontEn.FontFamily = new FontFamily(tempItem.Name);


                if(mFontItem!=null)
                {
                    mFontItem.Name = tempItem.Name;
                }
            }
        }

    }
}
