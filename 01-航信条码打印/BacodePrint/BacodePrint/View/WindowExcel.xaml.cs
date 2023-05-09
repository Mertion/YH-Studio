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
using System.Windows.Shapes;

using Microsoft.Win32;
using System.IO;
using BacodePrint.Fundation;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Drawing.Printing;
using NPOI.SS.Formula.Functions;
using System.Text.RegularExpressions;

namespace BacodePrint.View
{
    //行程单
    public class Itinerary
    {
        public List<string> CaptionList { get; set; } = new List<string>();
        public bool bCheck { get; set; } = false;
        //public string Caption_00 { get; set; } = "";
        //public string Caption_01 { get; set; } = "";
        //public string Caption_02 { get; set; } = ""; 
        //public string Caption_03 { get; set; } = "";
        //public string Caption_04 { get; set; } = "";
        //public string Caption_05 { get; set; } = "";
        //public string Caption_06 { get; set; } = "";
        //public string Caption_07 { get; set; } = "";
        //public string Caption_08 { get; set; } = "";
        //public string Caption_09 { get; set; } = "";
        //public string Caption_10 { get; set; } = "";
        //public string Caption_11 { get; set; } = "";
        //public string Caption_12 { get; set; } = "";
        //public string Caption_13 { get; set; } = "";
        //public string Caption_14 { get; set; } = "";
        //public string Caption_15 { get; set; } = "";
        //public string Caption_16 { get; set; } = "";
        //public string Caption_17 { get; set; } = "";
        //public string Caption_18 { get; set; } = "";
        //public string Caption_19 { get; set; } = "";
        //public string Caption_20 { get; set; } = "";
        //public string Caption_21 { get; set; } = "";
        //public string Caption_22 { get; set; } = "";
        //public string Caption_23 { get; set; } = "";
        //public string Caption_24 { get; set; } = "";
        //public string Caption_25 { get; set; } = "";
        //public string Caption_26 { get; set; } = "";
        //public string Caption_27 { get; set; } = "";
        //public string Caption_28 { get; set; } = "";
        //public string Caption_29 { get; set; } = "";
        //public string Caption_30 { get; set; } = "";
        //public string Caption_31 { get; set; } = "";
        //public string Caption_32 { get; set; } = "";
        //public string Caption_33 { get; set; } = "";
        //public string Caption_34 { get; set; } = "";
        //public string Caption_00 { get; set; } = "";
        //public string Caption_00 { get; set; } = "";
        //public string Caption_00 { get; set; } = "";
        //public string Caption_00 { get; set; } = "";
        //public string Caption_00 { get; set; } = "";
    }

    /// <summary>
    /// WindowExcel.xaml 的交互逻辑
    /// </summary>
    public partial class WindowExcel : Window
    {
        SystemGlobalInfo mSystemInfo = SystemGlobalInfo.Instance;
        //行程单列表
        //List<Itinerary> mItineraries = new List<Itinerary>();
        ObservableCollection<Itinerary> mItineraries = new ObservableCollection<Itinerary>();

        //行程单显示列表
        ObservableCollection<Itinerary> mItinerariesShow = new ObservableCollection<Itinerary>();
        public WindowExcel()
        {
            InitializeComponent();

            checkReserve.IsChecked = true;
            //ReadDataFromExcel();
        }

        //读Excel数据
        public int ReadDataFromExcel()
        {
            int nRet = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel文件|*.xls|所有文件|*.*";

            if((bool)openFileDialog.ShowDialog())
            {
                openFileDialog.FileName.ToString();
                ExcelHelper excelHelper = new ExcelHelper(openFileDialog.FileName);
                //读取数据
                excelHelper.GetExcelData("MySheet", true, ref mItineraries);

                Findlist();
                //gridItineraryList.ItemsSource = mItineraries;
            }
            else
            {
                nRet = 1;
            }

            return nRet;
        }

        //写Excel数据
        void WrtieDataToExcel()
        {
            ExcelHelper excelHelper = new ExcelHelper(@"I:\写入人员信息表.xlsx");
            //excelHelper.DataTableToExcel(dt, "MySheet", true);
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (gridItineraryList.Items.Count > 0)
            {
                for (int i = 0; i < gridItineraryList.Items.Count; i++)
                {
                    DataGridCell tCell = DataGridPlus.GetCell(gridItineraryList, i, 0);

                    if (tCell != null)
                    {
                        CheckBox checkBox = tCell.Content as CheckBox;
                        //checkBox.IsChecked = checkAll.IsChecked;
                    }
                    else
                    {
                        Debug.Print("tCell is null!");
                    }

                }
            }

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox checkBox = sender as CheckBox;
            //if (checkBox != null)
            //{
            //    var cntr = gridItineraryList.ItemContainerGenerator.ContainerFromIndex(gridItineraryList.SelectedIndex); //这里是拿到所选中行
            //    Itinerary selectItem = (Itinerary)(cntr as DataGridRow).DataContext; //这里是把选中行转换为对象,进而拿到CheckBox中绑定的名字

            //    foreach (var vItem in mItineraries)
            //    {

            //        //selectItem.bCheck = false; //这里是拿到MyList类中的isEnable属性 即上面 xmal中 Checkbox中绑定的变量


            //    }
            //}

            SumPrice();
        }

        //全选
        bool mbSellectAll = false;
        private void buttonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (!mbSellectAll)
            {
                foreach (Itinerary item in mItineraries)
                {
                    item.bCheck = true;
                }
                mbSellectAll = true;
            }
            else
            {
                foreach (Itinerary item in mItineraries)
                {
                    item.bCheck = false;
                }
                mbSellectAll = false;
            }

            gridItineraryList.ItemsSource = null;
            gridItineraryList.ItemsSource = mItineraries;

            SumPrice();
        }

        //反选
        private void buttonSelectInvert_Click(object sender, RoutedEventArgs e)
        {
            foreach (Itinerary item in mItineraries)
            {
                if (item.bCheck)
                {
                    item.bCheck = false;
                }
                else
                {
                    item.bCheck = true;
                }
            }

            gridItineraryList.ItemsSource = null;
            gridItineraryList.ItemsSource = mItineraries;

            SumPrice();
        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            //打印窗体
            PrintPage printPage = new PrintPage();
            if (printPage.ShowPrintDialog())
            {
                foreach (Itinerary item in mItineraries)
                {
                    if (item.bCheck)
                    {
                        List<string> tListTextPrint = new List<string>();
                        ExcelListToTemplateList(item.CaptionList, ref tListTextPrint);
                        printPage.SetData(item.CaptionList[0], tListTextPrint);
                        printPage.Show();
                        printPage.Print();
                        //printPage.Hide();
                    }
                }
            }

            printPage.Close();
        }

        private void ExcelListToTemplateList(List<string> pExcelList, ref List<string> pTemplateList)
        {
            const int const_nStep = 5;
            for(int i = 0; i < 5; i++)
            {
                pTemplateList.Add(pExcelList[i]);
            }

            //起飞航站楼-免费行李
            for(int i = 5; i < 17; i++)
            {
                SplitStringtolist(pExcelList[i], ref pTemplateList, const_nStep);
            }

            for (int i = 17; i < 31; i++)
            {
                pTemplateList.Add(pExcelList[i]);
            }
            SplitStringtolist(pExcelList[31], ref pTemplateList, 2);
            pTemplateList.Add(pExcelList[32]);
            pTemplateList.Add(pExcelList[33]);

            pTemplateList[66] = FormatPrice(pTemplateList[66]);

            pTemplateList[68] = FormatPrice(pTemplateList[68]);

            pTemplateList[70] = FormatPrice(pTemplateList[70]);

            pTemplateList[72] = FormatPrice(pTemplateList[72]);

            pTemplateList[74] = FormatPrice(pTemplateList[74]);
        }

        private string FormatPrice(string p_strSrc)
        {
            Regex re = new Regex("[^0-9.-]+");
            string str = "";
            if(re.IsMatch(p_strSrc))
            {
                str = p_strSrc;
            }
            else
            {
                //str = string.Format("{0:C2}", Convert.ToDouble( p_strSrc));
                if (str != "")
                {
                    str = Convert.ToDouble(p_strSrc).ToString("0.00"); 
                }
            }
            
            return str;
        }
        private void SumPrice()
        {
            double dSum = 0.0;
            foreach (Itinerary item in mItineraries)
            {
                if (item.bCheck)
                {
                    double dTval = item.CaptionList[26] == "" ? 0.0 : Convert.ToDouble(item.CaptionList[26]);
                    dSum += dTval;
                }
            }
            
            TextBoxSum.Text = string.Format("{0:C2}", dSum);
        }

        private void SplitStringtolist(String pStr,ref List<string> pTemplateList,int nStep)
        {
            int nCount= 0;
            string[] words = pStr.Split('|');
            
            foreach (var word in words)
            {
                pTemplateList.Add((string)word);
                nCount++;
            }

            for(int i = nCount; i<nStep;i++)
            {
                pTemplateList.Add("");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!mSystemInfo.mbExit)
            {
                this.Visibility = Visibility.Collapsed;
                e.Cancel = true;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            if(!(bool)checkReserve.IsChecked)
            {
                mItineraries.Clear();
            }

            ReadDataFromExcel();
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            //mItinerariesShow =;
            //mItineraries.CopyTo(mItinerariesShow.0);

            Findlist();
        }

        private void Findlist()
        {
            string str = textFind.Text;
            mItinerariesShow.Clear();

            str = str.Replace(" ", "");
            str = str.ToUpper();

            if (str == "")
            {
                for (int i = 0; i < mItineraries.Count; i++)
                {
                    mItinerariesShow.Add(mItineraries[i]);
                }
            }
            else
            {
                ObservableCollection<Itinerary> tItineraries = new ObservableCollection<Itinerary>();

                //生成临时数据列表
                foreach (var item in mItineraries)
                {
                    //var clone = (Itinerary)item.Clone();
                    var clone = (Itinerary)item;
                    tItineraries.Add(clone);
                }

                //在临时列表中逐个查找str中的每个字符是否在表中存在
                foreach (char c in str)
                {
                    //foreach (Itinerary item in tItineraries)
                    for (int i = 0; i < tItineraries.Count; )
                    {
                        bool bIsFind = false;
                        foreach(string strItem in tItineraries[i].CaptionList)
                        {
                            if(strItem.IndexOf(c)>=0)
                            {
                                bIsFind = true;
                                break;
                            }
                        }

                        if(bIsFind)
                        {
                            mItinerariesShow.Add(tItineraries[i]);
                            tItineraries.Remove(tItineraries[i]);
                            //因为剔除了一个item所以不-再++i;
                        }
                        else
                        {
                            i++;
                        }
                        //else
                        //{

                        //}
                    }
                }
            }

            //foreach (var item in mItineraries)
            //{
            //    //var clone = (Itinerary)item.Clone();

            //    var clone = (Itinerary)item;
            //    mItinerariesShow.Add(clone);
            //}

            gridItineraryList.ItemsSource = mItinerariesShow;
        }
    }



    public static class DataGridPlus
    {
        /// <summary>
        /// 获取DataGrid控件单元格
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">单元格所在的行号</param>
        /// <param name="columnIndex">单元格所在的列号</param>
        /// <returns>指定的单元格</returns>
        public static DataGridCell GetCell(this DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            DataGridRow rowContainer = dataGrid.GetRow(rowIndex);
            //DataGridRow rowContainer11 = dataGrid.GetRow(22);
            //DataGridCellsPresenter presenter11 = GetVisualChild<DataGridCellsPresenter>(rowContainer11);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                if (presenter != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    if (cell == null)
                    {
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[columnIndex]);
                        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    }
                    return cell;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// 获取DataGrid的行
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">DataGrid行号</param>
        /// <returns>指定的行号</returns>
        public static DataGridRow GetRow(this DataGrid dataGrid, int rowIndex)
        {
            DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (rowContainer == null)
            {
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }
            return rowContainer;
        }

        /// <summary>
        /// 获取父可视对象中第一个指定类型的子可视对象
        /// </summary>
        /// <typeparam name="T">可视对象类型</typeparam>
        /// <param name="parent">父可视对象</param>
        /// <returns>第一个指定类型的子可视对象</returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
