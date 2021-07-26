using System;
using System.Linq;
using Ganss.Excel;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class WeChatBillHandler : AbsBillHandler
    {
        public override ExportSuiBill GetExportSuiBill(string uploadFilePath)
        {
            var excelFile = new ExcelMapper(uploadFilePath)
            {
                HeaderRowNumber = 16,
                MinRowNumber = 16
            };
            excelFile.AddMapping<WeChatBill>( "交易时间", d => d.TransactionDate);
            excelFile.AddMapping<WeChatBill>( "交易类型", d => d.TransactionType);
            excelFile.AddMapping<WeChatBill>( "交易对方", d => d.Counterparty);
            excelFile.AddMapping<WeChatBill>( "商品", d => d.Merchandise);
            excelFile.AddMapping<WeChatBill>( "收/支", d => d.TradingBehavior);
            excelFile.AddMapping<WeChatBill>( "金额(元)", d => d.Amount);
            excelFile.AddMapping<WeChatBill>( "支付方式", d => d.PayMethod);
            excelFile.AddMapping<WeChatBill>( "当前状态", d => d.CurrentStatus);
            excelFile.AddMapping<WeChatBill>( "交易单号", d => d.TransactionNo);
            excelFile.AddMapping<WeChatBill>( "商户单号", d => d.MerchantNo);
            var dataList = excelFile.Fetch<WeChatBill>().ToList();
            excelFile.AddMapping<WeChatBill>( "备注", d => d.Remark);
            var groupDataList = dataList.GroupBy(x => x.TradingBehavior != "支出")
                .ToLookup(g => g.Key, g => g.ToList());
            
            var exportTemplate = new ExportSuiBill();

            foreach (var group in groupDataList)
            {
                // 收入
                if (group.Key)
                {
                    foreach (var item in group)
                    {
                        exportTemplate.Income = item.Select(x => new SuiTemplateBill
                        {
                            TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            Category = "其他收入",
                            SubCategory = "经营所得",
                            SourceAccount = "微信钱包",
                            Amount = decimal.Parse(x.Amount.Substring(1)),
                            Remark = $"{x.Counterparty}-{x.Merchandise}",
                        }).ToList();
                    }
                }

                // 支出
                else
                {
                    foreach (var item in group)
                    {
                        exportTemplate.Outgo = item.Select(x => new SuiTemplateBill
                        {
                            TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            Category = "其他杂项",
                            SubCategory = "其他支出",
                            SourceAccount = "微信钱包",
                            Amount = decimal.Parse(x.Amount.Substring(1)),
                            Remark = $"{x.Counterparty}-{x.Merchandise}",
                        }).ToList();
                    }
                }
            }

            return exportTemplate;
        }
    }
}