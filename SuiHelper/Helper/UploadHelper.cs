using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SuiHelper.Helper
{
    public class UploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public UploadHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<string> UploadFile(IFormFileCollection files)
        {
            var filePath = "";

            #region 保存文件

            var formFile = files.FirstOrDefault();
            if (formFile != null && formFile.Length > 0)
            { 
                string webRootPath = _webHostEnvironment.ContentRootPath;
                var fileName = formFile.FileName;
                string fileExt = fileName.Substring(fileName.LastIndexOf('.')); ; //文件扩展名，不含“.”
                long fileSize = formFile.Length; //获得文件大小，以字节为单位
                string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                filePath = webRootPath + "/Resource/" + newFileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            #endregion

            return filePath;
        }
    }
}