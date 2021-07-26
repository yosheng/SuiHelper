using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuiHelper.Common;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Services;

namespace SuiHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBillService _billService;

        public HomeController(ILogger<HomeController> logger, 
            IBillService billService)
        {
            _logger = logger;
            _billService = billService;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 将指定帐单类型转换为随手记模板并下载
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ActionResult> Convert(ConvertRequest req)
        {
            var path = await _billService.UploadBillExcel(req.File);
            var fileBuf = _billService.GetSuiBill(req.BillType, path);
            return File(fileBuf, "application/vnd.ms-excel", "随手记帐单" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd") + ".xls");
        }
        
        /// <summary>
        /// 获取导入帐单类型选单
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDropdownList()
        {
            var list = EnumHelper.GetEnumItem(typeof(BillType)).Select(x => new BillTypeItem()
            {
                Value = x.Key,
                Name = x.Value.Item1,
                Description = x.Value.Item2,
                ValidType = x.Value.Item3
            });
            return Json(list);
        }
    }
}