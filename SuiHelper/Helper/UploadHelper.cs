using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SuiHelper.Helper
{
    public static class UploadHelper
    {
        public static async Task<string> UploadFile(IFormFile file, string webPath)
        {
            var filePath = "";

            // 获取项目根目录下指定的文件下
            var relativePath = "/Resource/Excel";
            // 必须添加「~」才会指定到应用程序目录
            var webRootPath = Path.Combine(webPath, relativePath);

            // 如果不存在就创建file文件夹
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            var fileName = file.FileName;
            var fileExt = fileName.Substring(fileName.LastIndexOf('.')); ; //文件扩展名
            var fileSize = file.Length; //获得文件大小，以字节为单位
            string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
            filePath = webRootPath + "/" + newFileName;

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filePath;
        }
    }
}