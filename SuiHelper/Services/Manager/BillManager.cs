using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using SuiHelper.Helper;

namespace SuiHelper.Services.Manager
{
    public class BillManager : IBillManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BillManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public string GetExcelPathByCsvPath(string uploadFilePath, Encoding encoding)
        {
            var lines = FileHelper.ReadCsv(encoding, uploadFilePath, delimiter: ',');
            var workbook = FileHelper.ConvertWithNPOI(uploadFilePath, "Sheet1", lines);

            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Resource", "ConvertBillTemp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, $"{DateTime.Now:yyyy-MM-dd HHmmss}.xlsx");
            using var fs = File.OpenWrite(filePath);
            workbook.Write(fs);

            return filePath;
        }
    }
}