using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SuiHelper.Helper
{
    public static class CsvHelper
    {
        public static IEnumerable<string[]> ReadCsv(Encoding encoding, string fileName, char delimiter = ';')
        {
            var lines = System.IO.File.ReadAllLines(fileName, encoding).Select(a => a.Split(delimiter));
            return (lines);
        }
        
        public static IWorkbook ConvertWithNPOI(string excelFileName, string worksheetName, IEnumerable<string[]> csvLines)
        {
            if (csvLines == null || csvLines.Count() == 0)
            {
                throw new ArgumentException("找不到CSV档案");
            }

            int rowCount = 0;
            int colCount = 0;

            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet(worksheetName);

            foreach (var line in csvLines)
            {
                IRow row = worksheet.CreateRow(rowCount);

                colCount = 0;
                foreach (var col in line)
                {
                    row.CreateCell(colCount).SetCellValue(col);
                    colCount++;
                }
                rowCount++;
            }
            return workbook;
        }
    }
}