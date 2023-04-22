using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using BarcodeLib;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace BacodePrint
{
    class Template
    {

        public List<UserControlTextBoxItems> listText { get; set; } = new List<UserControlTextBoxItems>();
        public const int nListMaxCount = 34;

        public System.Windows.Controls.Image mImageBarcode = null;

        public Template()
        {
            for (int i = 0; i < Template.nListMaxCount; i++)
            {
                UserControlTextBoxItems UserControlTextBoxItems = new UserControlTextBoxItems();
                listText.Add(UserControlTextBoxItems);
            }

            mImageBarcode = new System.Windows.Controls.Image();
        }
    }

    internal class TemplateFundation
    {
        //加载模板控件到画布
        public void LoadTemplateToCanvas(Template p_Template, ref Canvas p_Canvas)
        {
            System.Windows.Controls.Image tImage = p_Template.mImageBarcode;
            p_Canvas.Children.Add(tImage);

            for (int i = 0; i < p_Template.listText.Count; i++)
            {
                UserControlTextBoxItems UserControlTextBoxItems = p_Template.listText[i];
                p_Canvas.Children.Add(UserControlTextBoxItems);
            }
        }

        public void GenerateBarCode(string p_StrBarcode, ref Template p_Template)
        {
            p_Template.mImageBarcode.Source = ClassBarCode.GenerateBarCodeBitmap(p_StrBarcode, ref p_Template.mImageBarcode);
        }

        public void SetTemplateData(string p_StrBarcode, List<string> p_strText,ref Template p_Template)
        {
            p_Template.mImageBarcode.Source = ClassBarCode.GenerateBarCodeBitmap(p_StrBarcode, ref p_Template.mImageBarcode);

            int nCount = p_strText.Count < p_Template.listText.Count ? p_strText.Count : p_Template.listText.Count;
            for(int i = 0;i<nCount;i++)
            {
                UserControlTextBoxItems UserControlTextBoxItems = p_Template.listText[i];
                UserControlTextBoxItems.SetString(p_strText[i]);
            }
        }
        //加载ini文件中的配置信息
        public void LoadTemplateFromIni(string p_strConfigFilePath, ref Template p_Template,ref Canvas p_Canvas, ref int p_nWidth, ref int p_nHeight)
        {
            IniFile tFilesINI = new IniFile();

            //读取模板长、宽两个数值
            {
                string str;
                str = tFilesINI.INIRead("Size", "Width", p_strConfigFilePath);
                p_nWidth = Convert.ToInt32(str);
                str = tFilesINI.INIRead("Size", "Height", p_strConfigFilePath);
                p_nHeight = Convert.ToInt32(str);
            }

            System.Windows.Controls.Image tImage = p_Template.mImageBarcode;
            {
                string str;
                var c = tImage as FrameworkElement;
                str = tFilesINI.INIRead("Barcode", "Left", p_strConfigFilePath);
                Canvas.SetLeft(c, Convert.ToInt32(str));

                str = tFilesINI.INIRead("Barcode", "Top", p_strConfigFilePath);
                Canvas.SetTop(c, Convert.ToInt32(str));

                str = tFilesINI.INIRead("Barcode", "Width", p_strConfigFilePath);
                c.Width = Convert.ToInt32(str);

                str = tFilesINI.INIRead("Barcode", "Height", p_strConfigFilePath);
                c.Height = Convert.ToInt32(str);
            }

            for (int i = 0; i < p_Template.listText.Count; i++)
            {
                string str;
                string strSetion = "Text" + i.ToString();

                UserControlTextBoxItems UserControlTextBoxItems = p_Template.listText[i];
                var c = UserControlTextBoxItems as FrameworkElement;

                //byte nColor = (byte)(i * 3);
                //Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x7f, 0x30, nColor));
                //UserControlTextBoxItems.Background = brush;

                str = tFilesINI.INIRead(strSetion, "FontFamily", p_strConfigFilePath);
                UserControlTextBoxItems.FontFamily = new System.Windows.Media.FontFamily(str);

                str = tFilesINI.INIRead(strSetion, "FontSize", p_strConfigFilePath);
                UserControlTextBoxItems.FontSize = Convert.ToInt32(str);

                str = tFilesINI.INIRead(strSetion, "RowSpacing", p_strConfigFilePath);
                UserControlTextBoxItems.mnRowSpacing = Convert.ToInt32(str);

                str = tFilesINI.INIRead(strSetion, "ColumnSpacing", p_strConfigFilePath);
                UserControlTextBoxItems.mnColumnSpacing = Convert.ToInt32(str);

                str = tFilesINI.INIRead(strSetion, "Left", p_strConfigFilePath);
                Canvas.SetLeft(c, Convert.ToInt32(str));

                str = tFilesINI.INIRead(strSetion, "Top", p_strConfigFilePath);
                Canvas.SetTop(c, Convert.ToInt32(str));

                str = tFilesINI.INIRead(strSetion, "Width", p_strConfigFilePath);
                c.Width = Convert.ToInt32(str);

                str = tFilesINI.INIRead(strSetion, "Height", p_strConfigFilePath);
                c.Height = Convert.ToInt32(str);

            }
        }

        public void SaveIni(string p_strConfigFilePath, Template p_Template, int p_nWidth, int p_nHeight)
        {
            IniFile tFilesINI = new IniFile();

            //保存模板长、宽两个数值
            {
                string str;
                str = p_nWidth.ToString();
                tFilesINI.INIWrite("Size", "Width", str, p_strConfigFilePath);
                //Height;
                str = p_nHeight.ToString();
                tFilesINI.INIWrite("Size", "Height", str, p_strConfigFilePath);
            }

            System.Windows.Controls.Image tImage = p_Template.mImageBarcode;
            {
                string str;
                string strSetion = "Barcode";
                var c = tImage as FrameworkElement;

                //Left
                str = Canvas.GetLeft(c).ToString();
                tFilesINI.INIWrite(strSetion, "Left", str, p_strConfigFilePath);
                //TOP
                str = Canvas.GetTop(c).ToString();
                tFilesINI.INIWrite(strSetion, "Top", str, p_strConfigFilePath);
                //Width
                str = c.Width.ToString();
                tFilesINI.INIWrite(strSetion, "Width", str, p_strConfigFilePath);
                //Height;
                str = c.Height.ToString();
                tFilesINI.INIWrite(strSetion, "Height", str, p_strConfigFilePath);
            }

            for (int i = 0; i < p_Template.listText.Count; i++)
            {
                string str;
                string strSetion = "Text" + i.ToString();
                UserControlTextBoxItems UserControlTextBoxItems = p_Template.listText[i];

                var c = UserControlTextBoxItems as FrameworkElement;
                //字体
                str = UserControlTextBoxItems.FontFamily.ToString();
                tFilesINI.INIWrite(strSetion, "FontFamily", str, p_strConfigFilePath);
                //字号
                str = UserControlTextBoxItems.FontSize.ToString();
                tFilesINI.INIWrite(strSetion, "FontSize", str, p_strConfigFilePath);
                //行间距
                str = UserControlTextBoxItems.mnRowSpacing.ToString();
                tFilesINI.INIWrite(strSetion, "RowSpacing", str, p_strConfigFilePath);
                //列间距
                str = UserControlTextBoxItems.mnColumnSpacing.ToString();
                tFilesINI.INIWrite(strSetion, "ColumnSpacing", str, p_strConfigFilePath);
                //Left
                str = Canvas.GetLeft(c).ToString();
                tFilesINI.INIWrite(strSetion, "Left", str, p_strConfigFilePath);
                //TOP
                str = Canvas.GetTop(c).ToString();
                tFilesINI.INIWrite(strSetion, "Top", str, p_strConfigFilePath);
                //Width
                str = c.Width.ToString();
                tFilesINI.INIWrite(strSetion, "Width", str, p_strConfigFilePath);
                //Height;
                str = c.Height.ToString();
                tFilesINI.INIWrite(strSetion, "Height", str, p_strConfigFilePath);
            }
        }
    }

}
