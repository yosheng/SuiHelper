using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using SuiHelper.Common;
using SuiHelper.Services;
using Xunit;

namespace SuiHelper.Tests.Services
{
    public class BillServiceTest : TestBase
    {
        private readonly IBillService _billService;

        public BillServiceTest()
        {
            _billService = ServiceProvider.GetRequiredService<IBillService>();
        }
        
        [Fact]
        public void ParseAbChinaBill()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", "AbChina.xls");
            _billService.GetSuiBill(BillType.AbChina ,path);
        }        
        
        [Fact]
        public void ParseIcbcBill()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", "Icbc.csv");
            _billService.GetSuiBill(BillType.Icbc ,path);
        }
    }
}