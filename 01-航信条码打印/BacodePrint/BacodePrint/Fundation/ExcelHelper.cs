using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using BacodePrint.Fundation;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace BacodePrint.Fundation
{
    internal class ExcelHelper : IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;
        public ExcelHelper(string fileName)//构造函数，读入文件名
        {
            this.fileName = fileName;
            disposed = false;
        }
        /// 将excel中的数据导入到DataTable中
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                workbook = WorkbookFactory.Create(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号，即总的列数
                    if (isFirstRowColumn)
                    {
                        data.Columns.Add(new DataColumn("选择"));
                        data.Columns.Add(new DataColumn("序号"));
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;//得到项标题后
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        dataRow[0] = false;
                        dataRow[1] = i.ToString();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j+2] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)//打印错误信息
            {
                MessageBox.Show("Exception: " + ex.Message);
                return null;
            }
        }

        //将DataTable数据导入到excel中
        //<param name="data">要导入的数据</param>
        //<param name="sheetName">要导入的excel的sheet的名称</param>
        //<param name="isColumnWritten">DataTable的列名是否要导入</param>
        //<returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        public void Dispose()//IDisposable为垃圾回收相关的东西，用来显式释放非托管资源,这部分目前还不是非常了解
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }
                fs = null;
                disposed = true;
            }
        }



        public void DaoChu()
        {
            //List<NeckDrugDetail> list = dgWareHouseDrug.ItemsSource as List<NeckDrugDetail>;
            //if (list.Count > 0)
            //{
            //    //Excel表格的创建步骤
            //    //第一步：创建Excel对象
            //    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //    //第二步：创建Excel对象的工作簿
            //    NPOI.SS.UserModel.ISheet sheet = book.CreateSheet();
            //    //第三步：Excel表头设置
            //    //给sheet添加第一行的头部标题
            //    NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(0);//创建行
            //    row1.CreateCell(0).SetCellValue("序号");
            //    row1.CreateCell(1).SetCellValue("药品名");
            //    row1.CreateCell(2).SetCellValue("数量");
            //    row1.CreateCell(3).SetCellValue("单位");
            //    row1.CreateCell(4).SetCellValue("零售价");
            //    row1.CreateCell(5).SetCellValue("零售金额");
            //    row1.CreateCell(6).SetCellValue("药品编号");
            //    row1.CreateCell(7).SetCellValue("规格");
            //    row1.CreateCell(8).SetCellValue("库存数");
            //    row1.CreateCell(9).SetCellValue("批号");
            //    //第四步：for循环给sheet的每行添加数据
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        NPOI.SS.UserModel.IRow row = sheet.CreateRow(i + 1);
            //        row.CreateCell(0).SetCellValue(i + 1);
            //        row.CreateCell(1).SetCellValue(list[i].DrugName);
            //        row.CreateCell(2).SetCellValue(list[i].count);
            //        row.CreateCell(3).SetCellValue(list[i].DrugUnitName);
            //        row.CreateCell(4).SetCellValue(list[i].RetailPrice.ToString());
            //        row.CreateCell(5).SetCellValue((double)list[i].RetailMoney);
            //        row.CreateCell(6).SetCellValue(list[i].DrugCode);
            //        row.CreateCell(7).SetCellValue(list[i].Specification);
            //        row.CreateCell(8).SetCellValue(list[i].InventoryCount);
            //        row.CreateCell(9).SetCellValue(list[i].PiHao);
            //    }

            //    //把Excel转化为文件流，输出
            //    SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Title = "选择要保存的路径";
            //    saveFileDialog.Filter = "Excel文件|*.xls|所有文件|*.*";
            //    saveFileDialog.FileName = string.Empty;
            //    saveFileDialog.FilterIndex = 1;
            //    saveFileDialog.RestoreDirectory = true;
            //    saveFileDialog.DefaultExt = "xls";
            //    saveFileDialog.CreatePrompt = true;

            //    if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        FileStream BookStream = new FileStream(saveFileDialog.FileName.ToString() + ".xls", FileMode.Create, FileAccess.Write);//定义文件流
            //        book.Write(BookStream);//将工作薄写入文件流                  
            //        BookStream.Seek(0, SeekOrigin.Begin); //输出之前调用Seek（偏移量，游标位置）方法：获取文件流的长度
            //        BookStream.Close();
            //    }
            //    else
            //    {
            //        MessageBox.Show("导出保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }

    }

}
