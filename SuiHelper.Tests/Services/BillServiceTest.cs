using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using SuiHelper.Common;
using SuiHelper.Services;
using SuiHelper.Services.Handler;
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
        
        [Theory]
        [InlineData("AbChina.xls", BillType.AbChina)]
        [InlineData("Icbc.csv", BillType.Icbc)]
        public void ParseBillFile(string fileName, BillType billType)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", fileName);
            _billService.GetSuiBill(billType ,path);
        }        
        
        [Theory]
        [InlineData("AbChina.xls", BillType.AbChina)]
        [InlineData("Icbc.csv", BillType.Icbc)]
        [InlineData("Cmb.csv", BillType.Cmb)]
        public void ParseSuiBill(string fileName, BillType billType)
        {
            var uploadFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "ExcelFiles", fileName);
            var exportTemplate = _billService.GetExportSuiBill(billType ,uploadFilePath);
            
            Assert.NotNull(exportTemplate);
        }
    }
}