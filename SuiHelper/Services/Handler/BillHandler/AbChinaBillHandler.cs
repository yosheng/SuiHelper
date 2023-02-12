using System;
using System.Linq;
using Ganss.Excel;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class AbChinaBillHandler : AbsBillHandler
    {
        public override ExportSuiBill GetExportSuiBill(string uploadFilePath)
        {
            var excelFile = new ExcelMapper(uploadFilePath) {HeaderRowNumber = 2, MinRowNumber = 2};
            excelFile.AddMapping<AbChinaBill>("交易时间", d => d.TransactionTime);
            excelFile.AddMapping<AbChinaBill>("交易金额", d => d.TransactionAmount);
            excelFile.AddMapping<AbChinaBill>("交易日期", d => d.TransactionDate);
            excelFile.AddMapping<AbChinaBill>("本次余额", d => d.CurrentBalance);
            excelFile.AddMapping<AbChinaBill>("对方户名", d => d.TransactionName);
            excelFile.AddMapping<AbChinaBill>("对方账号", d => d.TransactionAccount);
            excelFile.AddMapping<AbChinaBill>("交易行", d => d.TransactionBranch);
            excelFile.AddMapping<AbChinaBill>("交易渠道", d => d.TransactionChannel);
            excelFile.AddMapping<AbChinaBill>("交易类型", d => d.TransactionType);
            excelFile.AddMapping<AbChinaBill>("交易用途", d => d.TransactionPurpose);
            excelFile.AddMapping<AbChinaBill>("交易摘要", d => d.TradingSummary);

            var dataList = excelFile.Fetch<AbChinaBill>().ToList();

            var groupDataList = dataList.GroupBy(x => x.TransactionAmount >= 0)
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
                            TransactionDateTime = GetTransactionDateTime(x.TransactionDate, x.TransactionTime),
                            Category = "职业收入",
                            SubCategory = "利息收入",
                            SourceAccount = "中国农业银行",
                            Amount = x.TransactionAmount,
                            Remark = x.TradingSummary,
                        }).ToList();
                    }
                }

                // 支出或转帐
                else
                {
                    foreach (var item in group)
                    {
                        // 转帐: 找出支付宝或微信支付纪录
                        exportTemplate.Transfer = item
                            .Where(x => x.TradingSummary == "支付宝" || x.TradingSummary == "财付通" || x.TradingSummary == "微信支付").Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        GetTransactionDateTime(x.TransactionDate, x.TransactionTime),
                                    SourceAccount = "中国农业银行",
                                    TargetAccount = SuiTemplateBillHelper.GetTargetAccount(x.TradingSummary),
                                    Amount = Math.Abs(x.TransactionAmount),
                                }).ToList();

                        // 支出
                        exportTemplate.Outgo = item
                            .Where(x => x.TradingSummary != "支付宝" && x.TradingSummary != "财付通").Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        GetTransactionDateTime(x.TransactionDate, x.TransactionTime),
                                    Category = "其他杂项",
                                    SubCategory = "其他支出",
                                    SourceAccount = "中国农业银行",
                                    Amount = Math.Abs(x.TransactionAmount),
                                    Remark = $"{x.TradingSummary} {x.TransactionName}",
                                }).ToList();
                    }
                }
            }

            return exportTemplate;
        }
        
        private string GetTransactionDateTime(string transactionDate, string transactionTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(transactionTime))
                {
                    transactionTime = "000000";
                }
                
                return $"{transactionDate.Substring(0, 4)}-{transactionDate.Substring(4, 2)}-{transactionDate.Substring(6, 2)} {transactionTime.Substring(0, 2)}:{transactionTime.Substring(2, 2)}:{transactionTime.Substring(4, 2)}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"日期:{transactionDate} 时间:{transactionTime} 异常{e}");
                throw;
            }
        }
    }
}