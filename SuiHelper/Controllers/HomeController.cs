using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiHelper.Helper;
using SuiHelper.Models;

namespace SuiHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnviroment)
        {
            _logger = logger;
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ConvertWeChatBill()
        {
            var files = Request.Form.Files;
            var filePath = "";

            #region 保存CSV文件

            var formFile = files.FirstOrDefault();
            if (formFile != null && formFile.Length > 0)
            { 
                string webRootPath = _webHostEnviroment.ContentRootPath;
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
            
            var lines = CsvHelper.ReadCsv(Encoding.UTF8, filePath, delimiter: ',');
            var workbook = CsvHelper.ConvertWithNPOI(filePath, "NPOI", lines);
            var streams = new MemoryStream();
            workbook.Write(streams);
            var buf = streams.ToArray();
            return File(buf, "application/vnd.ms-excel", "订单信息" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd") + ".xlsx");
        }

        public async Task<IActionResult> UploadExcel()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _webHostEnviroment.ContentRootPath;
            foreach (var formFile in files)
            {
                
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;
                    string fileExt = fileName.Substring(fileName.LastIndexOf('.')); ; //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = webRootPath + "/Resource/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}