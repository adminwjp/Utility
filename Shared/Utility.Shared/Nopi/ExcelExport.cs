#if NET40 ||  NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETCOREAPP5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Utility.Nopi
{
    /// <summary>
    /// Excel常用的表格导出逻辑封装
    /// 单表写入
    /// </summary>
    public class ExcelExport
    {
        /// <summary>
        /// 导出的Excel文件名称+路径
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 导出的字段名称和描述
        /// </summary>
        public Dictionary<string, string> Fields { get; set; }

        private HSSFWorkbook _workbook = null;
        private ISheet _sheet = null;
        /// <summary>
        /// 创建实例，验证导出文件名
        /// </summary>
        /// <param name="FullName"></param>
        /// <param name="Fields"></param>
        public ExcelExport(string FullName, Dictionary<string, string> Fields)
        {
            this.FullName = FullName;
            this.Fields = Fields;
            Check();
            _workbook = new HSSFWorkbook();
            _sheet = _workbook.CreateSheet("Sheet1");
        }
        /// <summary>
        /// 验证Excel文件名
        /// </summary>
        private void Check()
        {
            try
            {
                FileInfo info = new FileInfo(this.FullName);
                string[] extentions = new string[] {
                ".xls",
                ".xlsx"
            };
                if (extentions.Any(q => q == info.Extension) == false)
                    throw new Exception("excel文件的扩展名不正确，应该为xls或xlsx");
                if (info.Exists == false)
                    info.Create().Close();
            }
            catch (Exception ex)
            {
                throw new Exception("创建Excel文件失败", ex);
            }
        }

        /// <summary>
        /// 执行导出操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void Export<T>(List<T> list)
        {
            //写入表格头
            WriteHead();
            //写入数据
            ICellStyle cellStyle = _workbook.CreateCellStyle();
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");//为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;

            IFont cellFont = _workbook.CreateFont();
            //cellFont.Boldweight = (short)FontBoldWeight.Normal;
            cellFont.IsBold = true;
            cellStyle.SetFont(cellFont);

            //建立行内容，从1开始
            int rowInex = 1;

            foreach (var rowItem in list)
            {
                //创建行
                IRow row = _sheet.CreateRow(rowInex);
                row.HeightInPoints = 25;

                int cellIndex = 0;
                foreach (var cellItem in this.Fields)
                {
                    //创建单元格
                    ICell cell = row.CreateCell(cellIndex);
                    //反射获取属性的值
                    PropertyInfo info = rowItem.GetType().GetProperty(cellItem.Key);
                    if (info == null)
                    {
                        cell.SetCellValue($"'{cellItem.Key}'属性不存在");
                    }
                    else
                    {
                        object value = info.GetValue(rowItem,null);
                        if (value != null)
                            cell.SetCellValue(value.ToString());
                    }
                    cell.CellStyle = cellStyle;
                    cellIndex++;
                }
                //进入下一次循环
                rowInex++;
            }

            //自适应列宽度
            for (int i = 0; i < this.Fields.Count; i++)
            {
                _sheet.AutoSizeColumn(i);
            }

            //导出到文件
            WriteFile();
        }
        /// <summary>
        /// 写入表头
        /// </summary>
        private void WriteHead()
        {
            //设置表头样式
            ICellStyle headStyle = _workbook.CreateCellStyle();
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.FillForegroundColor = HSSFColor.Blue.Index;
            headStyle.VerticalAlignment = VerticalAlignment.Center;

            IFont headFont = _workbook.CreateFont();
           // headFont.Boldweight = (short)FontBoldWeight.Bold;
            headFont.IsBold = true;
            headStyle.SetFont(headFont);

            IRow row = _sheet.CreateRow(0);
            row.HeightInPoints = 30;

            int index = 0;
            foreach (var item in this.Fields)
            {
                ICell cell = row.CreateCell(index);
                cell.SetCellValue(item.Value);
                cell.CellStyle = headStyle;
                index++;
            }
        }
        /// <summary>
        /// 创建文件到磁盘
        /// </summary>
        private void WriteFile()
        {
            using (FileStream fs = new FileStream(this.FullName, FileMode.OpenOrCreate))
            {
                _workbook.Write(fs);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
#endif