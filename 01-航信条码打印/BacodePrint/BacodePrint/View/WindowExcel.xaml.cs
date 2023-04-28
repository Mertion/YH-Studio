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

namespace BacodePrint.View
{
    //行程单
    public class Itinerary
    {
        public List<string> CaptionList { get; set; } = new List<string>();
    }

    /// <summary>
    /// WindowExcel.xaml 的交互逻辑
    /// </summary>
    public partial class WindowExcel : Window
    {
        DataTable dt;
        List<Itinerary> itineraries = new List<Itinerary>();
        public WindowExcel()
        {
            InitializeComponent();

            ReadDataFromExcel();
        }

        

        //读Excel数据
        void ReadDataFromExcel()
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Title = "选择要保存的路径";
            //    saveFileDialog.Filter = "Excel文件|*.xls|所有文件|*.*";
            //    saveFileDialog.FileName = string.Empty;
            //    saveFileDialog.FilterIndex = 1;
            //    saveFileDialog.RestoreDirectory = true;
            //    saveFileDialog.DefaultExt = "xls";
            //    saveFileDialog.CreatePrompt = true;

            //    if (saveFileDialog.ShowDialog() == DialogResult.OK)

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel文件|*.xls|所有文件|*.*";

            if((bool)openFileDialog.ShowDialog())
            {
                openFileDialog.FileName.ToString();
                //ExcelHelper excelHelper = new ExcelHelper(@"I:\人员信息表.xlsx");
                ExcelHelper excelHelper = new ExcelHelper(openFileDialog.FileName);
                //读取数据
                dt = excelHelper.ExcelToDataTable("MySheet", true);

                gridItineraryList.ItemsSource = dt.DefaultView;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    Itinerary itinerary = new Itinerary();
                //    itinerary.CaptionList.Add(dr[0].ToString());
                //    itinerary.CaptionList.Add(dr[1].ToString());

                //    itineraries.Add(itinerary);
                //}

                //this.gridItineraryList.ItemsSource = null;
                //this.gridItineraryList.ItemsSource = itineraries;
            }

        }

        //写Excel数据
        void WrtieDataToExcel()
        {
            ExcelHelper excelHelper = new ExcelHelper(@"I:\写入人员信息表.xlsx");
            excelHelper.DataTableToExcel(dt, "MySheet", true);
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
