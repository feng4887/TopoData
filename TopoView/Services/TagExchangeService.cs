//=============================================================================
// 对齐 TopoData：Public/ExcelHelper.cs（ExcelHelper、VALUE、EXP、EXP_DS）
//               与 Public/DataImport.cs 的点表导入
// Description: 点表与查询结果的 Excel 导入导出，保持旧版前六列顺序契约。
//=============================================================================
// Spreadsheet import/export shared by the device and history pages.
using System.Data;
using System.Globalization;
using auDASLib;
using ClosedXML.Excel;

namespace TopoView.Services;

/// <summary>
/// 点表与历史查询结果的 Excel 导入导出服务（ClosedXML 实现）。
/// </summary>
public static class TagExchangeService
{
    /// <summary>
    /// 导入点表 Excel：前六列依次为 TagId（点位短名称）、Address、DataType、UOM、
    /// Description、RW；重复点位或空地址直接报错。
    /// </summary>
    public static List<DBTag> ReadTags(string path, string channel)
    {
        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(1);
        var tags = new List<DBTag>();
        foreach (var row in sheet.RowsUsed().Skip(1))
        {
            var name = row.Cell(1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;
            var type = DBTag.GetDataType(row.Cell(3).GetString().Trim());
            if (type < 0) throw new InvalidDataException($"第 {row.RowNumber()} 行的数据类型无效。");
            tags.Add(new DBTag { CannelID = channel, TagName = name, TagId = channel + "." + name,
                Address = row.Cell(2).GetString().Trim(), DataType = type, UOM = row.Cell(4).GetString(),
                Description = row.Cell(5).GetString(), RW = row.Cell(6).TryGetValue<int>(out var rw) ? rw : 0 });
        }
        if (tags.GroupBy(t => t.TagId, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
            throw new InvalidDataException("导入表格包含重复点位。");
        if (tags.Any(t => string.IsNullOrWhiteSpace(t.Address))) throw new InvalidDataException("点位地址不能为空。");
        return tags;
    }

    /// <summary>导出点表为旧版六列结构的 Excel 文件。</summary>
    public static void WriteTags(string path, IEnumerable<DBTag> tags)
    {
        var table = new DataTable("Tags");
        foreach (var name in new[] { "TagId", "Address", "DataType", "UOM", "Description", "RW" }) table.Columns.Add(name);
        foreach (var tag in tags) table.Rows.Add(tag.TagName, tag.Address, DBTag.DataTypeString(tag.DataType), tag.UOM, tag.Description, tag.RW);
        WriteTable(path, table);
    }

    /// <summary>将 DataTable 写出为带标题行、冻结首行的 Excel 文件（历史查询导出共用）。</summary>
    public static void WriteTable(string path, DataTable table)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Data");
        for (var column = 0; column < table.Columns.Count; column++)
            sheet.Cell(1, column + 1).Value = table.Columns[column].ColumnName;
        for (var row = 0; row < table.Rows.Count; row++)
            for (var column = 0; column < table.Columns.Count; column++)
                sheet.Cell(row + 2, column + 1).Value = Convert.ToString(table.Rows[row][column], CultureInfo.InvariantCulture) ?? "";
        if (table.Columns.Count > 0)
        {
            sheet.Range(1, 1, 1, table.Columns.Count).Style.Fill.BackgroundColor = XLColor.FromHtml("#13243D");
            sheet.Range(1, 1, 1, table.Columns.Count).Style.Font.FontColor = XLColor.White;
            sheet.Columns(1, table.Columns.Count).Width = 22;
            sheet.SheetView.FreezeRows(1);
        }
        workbook.SaveAs(path);
    }
}
