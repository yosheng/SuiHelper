using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using SuiHelper.Common;
using SuiHelper.Models;

namespace SuiHelper.Helper
{
    public class SuiTemplateBillHelper
    {
        public static string GetValidFileTypeString(BillType billType)
        {
            var type = billType.GetType();
            var memInfo = type.GetMember(billType.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(ValidFileTypeAttribute), false);
            return (attributes.Length > 0) ? ((ValidFileTypeAttribute)attributes[0]).FileType : null;
        }

        public static string GetTargetAccount(string tradingSummary)
        {
            return tradingSummary switch
            {
                { } a when a.Contains("财付通") => "微信钱包",
                { } a when a.Contains("支付宝") => "支付宝",
                _ => ""
            };
        }
        
        public static byte[] GenerateExcelByte(ExportSuiBill exportTemplate)
        {
            var workbook = new HSSFWorkbook();
            
            #region 配置样式

            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.IsLocked = true;
            style.FillBackgroundColor = HSSFColor.Automatic.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = 12;

            var font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            font.Charset = 134;
            font.FontHeight = 220;
            font.Color = NPOI.HSSF.Util.HSSFColor.Green.Index;
            style.SetFont(font);
            
            #endregion

            SetExcelValue("支出", workbook.CreateSheet("支出"), exportTemplate.Outgo, style);
            SetExcelValue("收入", workbook.CreateSheet("收入"), exportTemplate.Income, style);
            SetExcelValue("转帐", workbook.CreateSheet("转帐"), exportTemplate.Transfer, style);
            
            var streams = new MemoryStream();
            workbook.Write(streams);

            return streams.ToArray();
        }

        private static void SetExcelValue(string transactionType, ISheet sheet, List<SuiTemplateBill> data, ICellStyle style)
        {
            var row = sheet.CreateRow(0);
            ICell cell;

            cell = row.CreateCell(0);
            cell.SetCellValue("交易类型");

            cell = row.CreateCell(1);
            cell.SetCellValue("日期");

            cell = row.CreateCell(2);
            cell.SetCellValue("分类");

            cell = row.CreateCell(3);
            cell.SetCellValue("子分类");

            cell = row.CreateCell(4);
            cell.SetCellValue("账户1");

            cell = row.CreateCell(5);
            cell.SetCellValue("账户2");

            cell = row.CreateCell(6);
            cell.SetCellValue("金额");

            cell = row.CreateCell(7);
            cell.SetCellValue("成员");

            cell = row.CreateCell(8);
            cell.SetCellValue("商家");

            cell = row.CreateCell(9);
            cell.SetCellValue("项目");

            cell = row.CreateCell(10);
            cell.SetCellValue("备注");
            
            foreach (var titleRowCell in row.Cells)
            {
                titleRowCell.CellStyle = style;
            }

            var rowIndex = 1;
            foreach (var item in data)
            {
                //创建一行，此行为第二行
                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(0);
                cell.SetCellValue(transactionType);

                cell = row.CreateCell(1);
                cell.SetCellValue(item.TransactionDateTime);

                cell = row.CreateCell(2);
                cell.SetCellValue(item.Category);

                cell = row.CreateCell(3);
                cell.SetCellValue(item.SubCategory);

                cell = row.CreateCell(4);
                cell.SetCellValue(item.SourceAccount);

                cell = row.CreateCell(5);
                cell.SetCellValue(item.TargetAccount);

                cell = row.CreateCell(6);
                cell.SetCellValue(item.Amount.ToString(CultureInfo.InvariantCulture));

                cell = row.CreateCell(7);
                cell.SetCellValue(item.Member);

                cell = row.CreateCell(8);
                cell.SetCellValue(item.Store);

                cell = row.CreateCell(9);
                cell.SetCellValue(item.Item);

                cell = row.CreateCell(10);
                cell.SetCellValue(item.Remark);

                rowIndex++;
            }
        }
    }
}