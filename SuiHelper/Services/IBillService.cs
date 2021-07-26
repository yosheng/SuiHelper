using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SuiHelper.Common;

namespace SuiHelper.Services
{
    public interface IBillService
    {
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