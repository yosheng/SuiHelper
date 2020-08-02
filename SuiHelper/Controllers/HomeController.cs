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
        private readonly UploadHelper _uploadHelper;
        

        public HomeController(ILogger<HomeController> logger, UploadHelper uploadHelper)
        {
            _logger = logger;
            _uploadHelper = uploadHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ConvertWeChatBill()
        {
            var filePath = await _uploadHelper.UploadFile(Request.Form.Files);
            
            var lines = CsvHelper.ReadCsv(Encoding.UTF8, filePath, delimiter: ',');
            var workbook = CsvHelper.ConvertWithNPOI(filePath, "NPOI", lines);
            var streams = new MemoryStream();
            workbook.Write(streams);
            var buf = streams.ToArray();
            return File(buf, "application/vnd.ms-excel", "订单信息" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd") + ".xlsx");
        }

        public async Task<IActionResult> UploadExcel()
        {
            await _uploadHelper.UploadFile(Request.Form.Files);
            return Ok("上传成功");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}