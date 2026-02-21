using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Documents;

/****************************************************************
* 标题: Excel作类库
* 描述: 
* 作者: 杜金旺 
* 日期:2025-12-25
* *************************************************************/
namespace auDASLib

{
    public class VALUE
    {
        public VALUE()
        {

        }

        public int irow = 0;
        public int icol = 0;
        public object ovalue = null;
    }

    public class EXP
    {
        public List<VALUE> ValueList = new List<VALUE>();
        public string InputRoot;
        public string OutputRoot;

    }

    public class EXP_DS
    {
        public DataSet ds = new DataSet();
        public List<VALUE> ValueList = new List<VALUE>();
        public string InputRoot;
        public string OutputRoot;
        public int startrow = 1;
        public int startcol = 1;

        /// <summary>
        /// 有标题，datatable 生效
        /// </summary>
        public bool WithHeader = false;

        /// <summary>
        /// Whether to freeze panes, WitherHeader为true时生效
        /// </summary>
        public bool FreezePanes = false;
    }

    public class ExcelHelper
    {
        #region Read
        /// <summary>
        /// Import Excel file
        /// </summary>
        /// <param name="fileName">Full file name</param>
        /// <returns></returns>
        public static DataTable Import(string fileName)
        {

            //using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            //{
            //    // Auto-detect format, supports:
            //    //  - Binary Excel files (2.0-2003 format; *.xls)
            //    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            //    using (var reader = ExcelReaderFactory.CreateReader(stream))
            //    {
            //        // Choose one of either 1 or 2:

            //        // 1. Use the reader methods
            //        do
            //        {
            //            while (reader.Read())
            //            {
            //                //reader.GetDouble(0);
            //                var a = reader.GetValue(0);
            //                var b = reader.GetValue(1);
            //                // reader.GetDouble(0);
            //            }
            //        } while (reader.NextResult());


            //        //// 2. Use the AsDataSet extension method
            //        //var result = reader.AsDataSet();

            //        //reader.

            //        // The result of each spreadsheet is in result.Tables
            //    }
            //}

            //    FileInfo existingFile = new FileInfo();
            //using (ExcelPackage package = new ExcelPackage(existingFile))
            //{
            //    //get the first worksheet in the workbook
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            //    int colCount = worksheet.Dimension.End.Column;  //get Column Count
            //    int rowCount = worksheet.Dimension.End.Row;     //get row count
            //    for (int row = 1; row <= rowCount; row++)
            //    {
            //        for (int col = 1; col <= colCount; col++)
            //        {
            //            Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value.ToString().Trim());
            //        }
            //    }
            //}

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var dt = new DataTable();
            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {
                int m = 1;

                var sheet = excel.Workbook.Worksheets.First();
                if (sheet == null)
                {
                    return null;
                }
                foreach (var cell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    //dt.Columns.Add(cell.Value.ToString());
                    dt.Columns.Add(m++.ToString());
                }

                if(dt.Columns.Count < 3)
                    dt.Columns.Add("3");

                if (dt.Columns.Count < 4)
                    dt.Columns.Add("4");

                if (dt.Columns.Count < 5)
                    dt.Columns.Add("5");

                var rows = sheet.Dimension.End.Row;
                //for (var i = 1; i < rows; i++)
                //{
                //    var row = sheet.Cells[i + 1, 1, i + 1, sheet.Dimension.End.Column];
                //    dt.Rows.Add(row..ToArray());
                //}
                //for (var i = 1; i < rows; i++)
                //{                   
                //    var row = sheet.Cells[i + 1, 1, i + 1, sheet.Dimension.End.Column];
                //    dt.Rows.Add(row.Select(cell => cell.Value).ToArray());
                //}
                //----正确的
                int n = sheet.Dimension.End.Column;
                for (var i = 1; i < rows + 1; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (var j = 0; j < n; j++)
                    {
                        if (sheet.Cells[i, j + 1, i, j + 1].Value == null)
                        {
                            dr[j] = "";
                        }
                        else { dr[j] = sheet.Cells[i, j + 1, i, j + 1].Value.ToString(); }
                    }
                    dt.Rows.Add(dr);
                }
                //^正确的
                return dt;
            }


        }

        #endregion //Read

        #region Export

        private string m_strInputDirectory = "";
        private string m_strOutputDirectory = "";

        List<VALUE> m_ValueList = new List<VALUE>();
        DataSet m_DataSet = new DataSet();

        public string InputRoot
        {
            set { m_strInputDirectory = value; }
            get { return m_strInputDirectory; }

        }

        public string OutputRoot
        {
            set { m_strOutputDirectory = value; }
            get { return m_strOutputDirectory; }
        }

        public DataSet Ds
        {
            set { m_DataSet = value; }
            get { return m_DataSet; }
        }

        public void Add(int row, int col, object value)
        {
            VALUE va = new VALUE();
            va.icol = col;
            va.irow = row;
            va.ovalue = value;
            m_ValueList.Add(va);
        }

        /// <summary>
        /// Out put List Items
        /// </summary>
        public void SaveList()
        {
            Thread t = new Thread(new ParameterizedThreadStart(savebyList));
            EXP exp = new EXP();
            exp.InputRoot = InputRoot;
            exp.OutputRoot = OutputRoot;
            //exp.OutputRoot = saveFileDialog1.FileName;
            exp.ValueList = m_ValueList;
            t.Start(exp);
        }

        private void savebyList(object ob)
        {
            EXP exp = ob as EXP;
            try
            {
                FileInfo existingFile = new FileInfo(exp.InputRoot);
                FileInfo OutPutFile = new FileInfo(exp.OutputRoot);
                if (OutPutFile.Exists)
                {
                    OutPutFile.Delete();  // ensures we create a new workbook
                    OutPutFile = new FileInfo(OutputRoot);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(OutPutFile, existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    foreach (VALUE va in exp.ValueList)
                    {
                        worksheet.Cells[va.irow, va.icol].Value = va.ovalue;
                    }
                    package.Save();
                }
                m_ValueList.Clear();
            }
            catch
            {

            }
        }

        public bool SaveDs(int startrow = 1,int startcol = 1, bool withHeader = false, bool FreezePanes = false)
        {
            EXP_DS exp = new EXP_DS();
            exp.InputRoot = InputRoot;
            exp.OutputRoot = OutputRoot;
            exp.startcol = startcol;
            exp.startrow = startrow;
            exp.ValueList = m_ValueList;
            exp.ds = Ds;
            exp.WithHeader = withHeader;  // 有header
            exp.FreezePanes = FreezePanes;// 冻结窗格
            savebyDataset(exp);
            return true;
        }

        public bool SaveDsWithHeader(bool FreezePanes = false,int startrow = 1, int startcol = 1 )
        {
            return SaveDs(startrow, startcol, true,true);
        }

        private void savebyDataset(object ob)
        {
            EXP_DS exp = ob as EXP_DS;
            int rowCount = exp.ds.Tables[0].Rows.Count;		//DataTable行数
            int colCount = exp.ds.Tables[0].Columns.Count;	//DataTable列数

            try
            {
                FileInfo existingFile = null;
                if (!string.IsNullOrEmpty(exp.InputRoot))
                {
                    existingFile = new FileInfo(exp.InputRoot);
                }

                FileInfo OutPutFile = null;
                if (!string.IsNullOrEmpty(exp.OutputRoot))
                {
                    OutPutFile = new FileInfo(exp.OutputRoot);
                }

                if (OutPutFile.Exists)
                {
                    OutPutFile.Delete();  // ensures we create a new workbook
                    OutPutFile = new FileInfo(OutputRoot);
                }

                if (!string.IsNullOrEmpty(exp.InputRoot))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(OutPutFile, existingFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                        // ① 写表头
                        int rowAppend = 0;
                        if (exp.WithHeader)
                        {
                            WriteTableHeader(worksheet, exp.ds.Tables[0], exp.startrow, exp.startcol);
                            rowAppend = 1;
                        }

                        // ② 写数据
                        for (int j = 0; j < rowCount; j++)
                        {
                            for (int k = 0; k < colCount; k++)
                            {
                                worksheet.Cells[exp.startrow + j + rowAppend, exp.startcol + k].Value = exp.ds.Tables[0].Rows[j][k];
                            }
                        }

                        foreach (VALUE va in exp.ValueList)
                        {
                            worksheet.Cells[va.irow, va.icol].Value = va.ovalue;
                        }

                        //自动调整列宽
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //冻结窗格
                        if (exp.WithHeader && exp.FreezePanes)
                            worksheet.View.FreezePanes(exp.startrow + 1, exp.startcol);

                        package.Save();
                    }
                }
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(OutPutFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");
                        //ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                        // ① 写表头
                        int rowAppend = 0;
                        if (exp.WithHeader)
                        { 
                            WriteTableHeader(worksheet, exp.ds.Tables[0], exp.startrow, exp.startcol);
                            rowAppend = 1;

                        }

                        // ② 写数据
                        for (int j = 0; j < rowCount; j++)
                        {
                            for (int k = 0; k < colCount; k++)
                            {
                                worksheet.Cells[exp.startrow + j + rowAppend, exp.startcol + k].Value = exp.ds.Tables[0].Rows[j][k];
                            }
                        }

                        foreach (VALUE va in exp.ValueList)
                        {
                            worksheet.Cells[va.irow, va.icol].Value = va.ovalue;
                        }

                        //自动调整列宽
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //冻结窗格
                        if(exp.WithHeader && exp.FreezePanes)
                            worksheet.View.FreezePanes(exp.startrow + 1, exp.startcol);

                        package.Save();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void WriteTableHeader(ExcelWorksheet worksheet, DataTable table, int headerRow, int startCol)
        {
            for (int k = 0; k < table.Columns.Count; k++)
            {
                worksheet.Cells[headerRow, startCol + k].Value = table.Columns[k].ColumnName;

                // 可选：简单表头样式
                // 粗表头
                worksheet.Cells[headerRow, startCol + k].Style.Font.Bold = true;

                //填充颜色
                worksheet.Cells[headerRow, startCol + k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[headerRow, startCol + k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
            }
        }

        //public bool SaveDs(int startrow = 1, int startcol = 1)
        //{
        //    EXP_DS exp = new EXP_DS();
        //    exp.InputRoot = InputRoot;
        //    exp.OutputRoot = OutputRoot;
        //    exp.startcol = startcol;
        //    exp.startrow = startrow;
        //    exp.ValueList = m_ValueList;
        //    exp.ds = Ds;

        //    savebyDataset(exp);
        //    return true;
        //}
        #endregion //Export
    }
}