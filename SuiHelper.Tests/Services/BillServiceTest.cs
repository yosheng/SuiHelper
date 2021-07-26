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
        private readonly BillService _billService;

        public BillServiceTest()
        {
            _billService = ServiceProvider.GetRequiredService<BillService>();
        }
        
        [Fact]
        public void Should_GetSuiBill()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", "AbChina.xls");
            _billService.GetSuiBill(BillType.AbChina ,path);
        }
    }
}