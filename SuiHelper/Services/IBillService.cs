using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SuiHelper.Common;
using SuiHelper.Models;

namespace SuiHelper.Services
{
    public interface IBillService
    {
        /// <summary>
        /// 获取随手记实体
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="uploadFilePath"></param>
        /// <returns></returns>
        ExportSuiBill GetExportSuiBill(BillType billType, string uploadFilePath);
        
        /// <summary>
        /// 获取随手记帐单档案流
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="uploadFilePath"></param>
        /// <returns></returns>
        byte[] GetSuiBill(BillType billType, string uploadFilePath);
        
        Task<string> UploadBillExcel(IFormFile file);
    }
}