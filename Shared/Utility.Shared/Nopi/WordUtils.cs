#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETCOREAPP5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility.Nopi
{
    /// <summary>
    /// <para>Microsoft.Office.Interop.Word</para>
    /// <para>NPOI</para>
    /// </summary>
    public class WordUtils
    {

      /// <summary>
      /// 
      /// </summary>
      /// <param name="wordEntities"></param>
        public static void Create(List<WordEntity> wordEntities)
        {
            //创建生成word文档
            string path = "D:\\test.docx";
            NOPIWordFactory wordFactory = new NOPIWordFactory();
            XWPFDocument doc = new XWPFDocument();
            foreach (var item in wordEntities)
            {
                wordFactory.AddP(item);
            }

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                doc.Write(fs);
                Console.WriteLine("生成word成功");
            }
        }
        public class NOPIWordFactory
        {
            public XWPFDocument Document { get; set; } = new XWPFDocument();
            //添加表格
            public XWPFTable Table { get; set; }
            /// <summary>
            /// 添加段落
            /// </summary>
            /// <param name="wordEntity"></param>
            public void AddP(WordEntity wordEntity)
            {
                // 添加段落
                var gp = Document.CreateParagraph();
                switch (wordEntity.Align)
                {
                    case WordAlign.None:
                        break;
                    case WordAlign.Left:
                        gp.Alignment = ParagraphAlignment.LEFT;
                        break;
                    case WordAlign.Right:
                        gp.Alignment = ParagraphAlignment.RIGHT;
                        break;
                    case WordAlign.Center:
                        gp.Alignment = ParagraphAlignment.CENTER;//水平居中
                        break;
                    default:
                        break;
                }

                var gr = gp.CreateRun();
                var rpr = gr.GetCTR().AddNewRPr();
                var font = rpr.AddNewRFonts();
                var fontStr = wordEntity.Font.ToString();
                font.ascii = fontStr;
                font.eastAsia = fontStr;
                font.hint = ST_Hint.eastAsia;
                var sz = rpr.AddNewSz();
                sz.val = (ulong)wordEntity.Size;
                sz.val = (ulong)wordEntity.Size;
                rpr.AddNewB().val = wordEntity.Bold; //加粗
                rpr.AddNewColor().val = wordEntity.Color.ToString().ToLower();//字体颜色
                gr.SetText(wordEntity.Text);
            }
            /// <summary>
            /// 添加表格
            /// </summary>
            public void AddTable(WordExcelRowEntity[] rowEntities)
            {
                //添加表格
                XWPFTable table = Document.CreateTable(rowEntities.Length, rowEntities[0].Cells.Count);//创建 rowEntities.Length 行 rowEntities[0].Cells.Count 列表
                this.Table = table;
                for (int i = 0; i < rowEntities.Length; i++)
                {
                    for (int j = 0; j < rowEntities[0].Cells.Count; j++)
                    {
                        var cell = table.GetRow(i).GetCell(j);
                        //table.GetRow(i).MergeCells();
                        cell.SetText(rowEntities[0].Cells[j].Value);
                        cell.SetVerticalAlignment( XWPFTableCell.XWPFVertAlign.CENTER);
                        
                    }
                }
            }
            public void AddTableRow(string[] cells)
            {
                this.Table = this.Table ?? Document.CreateTable();
                CT_Row m_NewRow = new CT_Row();//创建1行
                XWPFTableRow m_Row = new XWPFTableRow(m_NewRow, Table);
                Table.AddRow(m_Row); //必须要！！！
                for (int i = 0; i < cells.Length; i++)
                {
                    XWPFTableCell cell = m_Row.CreateCell();//创建单元格，也创建了一个CT_P
                    CT_Tc cttc = cell.GetCTTc();
                    //CT_TcPr ctPr = cttc.AddNewTcPr();
                    //ctPr.gridSpan.val = "3";//合并3列
                    cttc.GetPList()[0].AddNewPPr().AddNewJc().val = ST_Jc.center;
                    cttc.GetPList()[0].AddNewR().AddNewT().Value = "666";
                }
            }
            public void SetTabelPro()
            {
                CT_Tbl m_CTTbl = Document.Document.body.GetTblArray()[0];//获得文档第一张表
                CT_TblPr m_CTTblPr = m_CTTbl.AddNewTblPr();
                m_CTTblPr.AddNewTblW().w = "2000"; //表宽
                m_CTTblPr.AddNewTblW().type = ST_TblWidth.dxa;
                m_CTTblPr.tblpPr = new CT_TblPPr();//表定位
                m_CTTblPr.tblpPr.tblpX = "4003";//表左上角坐标
                m_CTTblPr.tblpPr.tblpY = "365";
                m_CTTblPr.tblpPr.tblpXSpec = ST_XAlign.center;//若不为“Null”，则优先tblpX，即表由tblpXSpec定位
                m_CTTblPr.tblpPr.tblpYSpec = ST_YAlign.center;//若不为“Null”，则优先tblpY，即表由tblpYSpec定位  
                m_CTTblPr.tblpPr.leftFromText = (ulong)180;
                m_CTTblPr.tblpPr.rightFromText = (ulong)180;
                m_CTTblPr.tblpPr.vertAnchor = ST_VAnchor.text;
                m_CTTblPr.tblpPr.horzAnchor = ST_HAnchor.page;
            }
        }
    }
    public enum FontColor
    {
        Red,
    }
    public enum FontSize : ulong
    {
        /// <summary>
        /// 2号字体
        /// </summary>
        Second = 44,
        /// <summary>
        /// 5号字体
        /// </summary>
        Five = 21
    }
    public class WordEntity
    {
        public WordFont Font { get; set; }
        public WordAlign Align { get; set; }
        public FontSize Size { get; set; }
        /// <summary>
        /// 字体是否加粗
        /// </summary>
        public bool Bold { get; set; }
        public FontColor Color { get; set; }
        /// <summary>
        ///文本
        /// </summary>
        public string Text { get; set; }
        
    }
    public class WordExcelEntity
    {

    }
    public class WordExcelRowEntity
    {
        public List<WordExcelCellEntity> Cells { get; set; }
    }
    public class WordExcelCellEntity
    {
       public string Value { get; set; }
        public WordAlign Align { get; set; } = WordAlign.Left;
    }
    public enum WordFont
    {
        黑体,
        宋体
    }
    public enum WordAlign
    {
        None,
        Left,
        Right,
        Center
    }
}
#endif