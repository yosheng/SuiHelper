using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SuiHelper.Common;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Services.Handler;
using SuiHelper.Services.Manager;

namespace SuiHelper.Services
{
    public class BillService : IBillService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBillManager _billManager;

        public BillService(IWebHostEnvironment webHostEnvironment, IBillManager billManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _billManager = billManager;
        }

        public ExportSuiBill GetExportSuiBill(BillType billType, string uploadFilePath)
        {
            if (SuiTemplateBillHelper.GetValidFileTypeString(billType).Equals("csv"))
            {
                if (billType == BillType.Cmb)
                {
                    uploadFilePath = _billManager.GetExcelPathByCsvPath(uploadFilePath, Encoding.GetEncoding("GB2312"));
                }
                else if(billType == BillType.AliPay)
                {
                    uploadFilePath = uploadFilePath;
                }
                else
                {
                    uploadFilePath = _billManager.GetExcelPathByCsvPath(uploadFilePath, Encoding.UTF8);
                }
            }
            
            return ExportSuiBillFactory.CreateBillHandler(billType).GetExportSuiBill(uploadFilePath);
        }
        
        public byte[] GetSuiBill(BillType billType, string uploadFilePath)
        {
            var exportTemplate = GetExportSuiBill(billType, uploadFilePath);
            
            return SuiTemplateBillHelper.GenerateExcelByte(exportTemplate);
        }
        
        public async Task<string> UploadBillExcel(IFormFile file)
        {
            // 获取项目根目录下指定的文件下
            var relativePath = "/Resource/Excel";
            // 必须添加「~」才会指定到应用程序目录
            var webRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, relativePath);

            // 如果不存在就创建file文件夹
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            var fileName = file.FileName;
            var fileExt = fileName.Substring(fileName.LastIndexOf('.'));  //文件扩展名
            var newFileName = Guid.NewGuid() + fileExt; //随机生成新的文件名
            var filePath = webRootPath + "/" + newFileName;

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }
    }
}