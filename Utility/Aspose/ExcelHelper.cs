#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using Aspose.Cells;
using Aspose.Cells.Rendering;
using System.Drawing;
using System.IO;
using Aspose.Cells.Drawing;
using System.Collections.Generic;
using System;
using Utility.IO;
using System.Reflection;
using System.Collections;
using Utility.Utils;

namespace Utility.Aspose
{

    /// <summary>
    /// 
    /// </summary>
    public class ExcelHelper
    {

        /// <summary>
        /// 写入 excel
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="title">标题</param>
        /// <param name="xlsx"></param>
        /// <param name="merge">嵌套实体 合并 </param>
        public static void Write(object obj,string title,string xlsx,bool merge=true)
        {
            Workbook workbook = File.Exists(xlsx)? new Workbook(xlsx): new Workbook();
            // int count=workbook.Worksheets.Count;
            Worksheet worksheet = workbook.Worksheets[title] ?? workbook.Worksheets.Add(title);

            //不做缓存 直接 反射 写入 嵌套 多层 怎么 反射 了 
            //简单 基本 类型 写入
            //这一步 必须 做 缓存 啊 不然 数据 乱七八糟 写入 可能 不对
            CursionWrite(worksheet,obj);
       
            workbook.Save(xlsx);
        }

        /// <summary>
        /// 写入 excel
        /// </summary>
        /// <param name="worksheet">excel</param>
        /// <param name="obj">实体</param>
        /// <param name="row">行</param>
        /// <param name="cell"></param>
        private static void CursionWrite(Worksheet worksheet ,object obj, int row=1, int cell=0)
        {
            //不做缓存 直接 反射 写入 嵌套 多层 怎么 反射 了 
            //简单 基本 类型 写入
            //集合
            if (obj is IEnumerable enumerable)
            {
                IEnumerator iterator = enumerable.GetEnumerator();
                while (iterator.MoveNext())
                {
                    int c = cell;
                    CursionWrite(worksheet, iterator.Current,  row, cell);
                    row++;
                }
                if (iterator is IDisposable disposable)
                {
                    disposable.Dispose();//释放资源
                }
              
                return;
            }
            else if (obj is IDictionary dictionary)
            {
                IDictionaryEnumerator dictionaryEnumerator = dictionary.GetEnumerator();
                while (dictionaryEnumerator.MoveNext())
                {
                    CursionWrite(worksheet, dictionaryEnumerator.Value,  row, cell);
                    row++;
                }
                if (dictionaryEnumerator is IDisposable disposable)
                {
                    disposable.Dispose();//释放资源
                }
           
                return;
            }
            Type type = obj.GetType();
            if (!TypeUtils.IsCommonType(type))
            {
                //单
                foreach (PropertyInfo property in type.GetProperties())
                {
                    worksheet.Cells[0, cell].Value = worksheet.Cells[0, cell].Value ?? property.Name;//标题 
                                                                                                     //先 直接 插入 后面 遇到关联 则 合并 单元格 放弃 合并 不然 都需要移动
                    if (!TypeUtils.IsCommonType(property.PropertyType))
                    {
                        object val = property.GetValue(obj,null);
                        if (val != null)
                        {
                            //断了 话 标题 没有 
                            //递归  如果 循环 引用 了
                            CursionWrite(worksheet, val, row, cell);
                        }
                    }
                    else
                    {
                        worksheet.Cells[row, cell].Value = property.GetValue(obj,null);//值
                    }
                    cell++;//断了 话 不对
                }
            }
        }
        /// <summary>
        /// 读取 excel
        /// </summary>
        /// <param name="xlsx"></param>
        public static void Read(string xlsx)
        {
            Workbook workbook = new Workbook(xlsx);
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                Cells cells = worksheet.Cells;
                for (int i = 0; i < cells.MaxDataRow + 1; i++)
                {
                    for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                    {
                        string s = cells[i, j].StringValue.Trim();
                        //一行行的读取数据，插入数据库的代码也可以在这里写
                    }
                }
            }

        }

        /// <summary>把Aspose.Cells.Drawing.Picture对象转换为Image对象 </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static  Image ChangeToImage(Picture pic)
        {
            Bitmap img = new Bitmap(ChangeToStream(pic));
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Image ChangeToImage(Stream stream)
        {
            Bitmap img = new Bitmap(stream);
            return img;
        }

        /// <summary>把Aspose.Cells.Drawing.Picture对象转换为Stream对象 </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static  Stream ChangeToStream(Picture pic)
        {
            ImageOrPrintOptions printOption = new ImageOrPrintOptions(); //图片格式
            printOption.ImageType = pic.ImageType;
            //printOption.ImageFormat = pic.ImageFormat;
            MemoryStream mstream = new MemoryStream();
            pic.ToImage(mstream, printOption);  // 保存（参数为：内存流和图片格式）
            return mstream;
        }
    }
}
#endif