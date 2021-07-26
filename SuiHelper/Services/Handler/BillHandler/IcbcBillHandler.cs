using System;
using System.Linq;
using Ganss.Excel;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class IcbcBillHandler : AbsBillHandler
    {
        public override ExportSuiBill GetExportSuiBill(string uploadFilePath)
        {
            var excelFile = new ExcelMapper(uploadFilePath)
            {
                HeaderRowNumber = 6,
                MinRowNumber = 6
            };
            excelFile.AddMapping<IcbcBill>("交易日期", d => d.TransactionDate);
            excelFile.AddMapping<IcbcBill>("摘要", d => d.Summary);
            excelFile.AddMapping<IcbcBill>("交易场所", d => d.Description);
            excelFile.AddMapping<IcbcBill>("交易国家或地区简称", d => d.Region);
            excelFile.AddMapping<IcbcBill>("钞/汇", d => d.Unit);
            excelFile.AddMapping<IcbcBill>("交易金额(收入)", d => d.TransactionIncome);
            excelFile.AddMapping<IcbcBill>("交易金额(支出)", d => d.TransactionOutCome);
            excelFile.AddMapping<IcbcBill>("交易币种", d => d.TransactionCurrency);
            excelFile.AddMapping<IcbcBill>("记账金额(收入)", d => d.Income);
            excelFile.AddMapping<IcbcBill>("记账金额(支出)", d => d.Outcome);
            excelFile.AddMapping<IcbcBill>("记账币种", d => d.Currency);
            excelFile.AddMapping<IcbcBill>("余额", d => d.AccountingCurrencyBalance);
            excelFile.AddMapping<IcbcBill>("对方户名", d => d.TransactionAccount);
            var dataList = excelFile.Fetch<IcbcBill>().ToList();

            #region 处理元数据

            dataList.ForEach(x =>
            {
                x.TransactionDate = TrimContent(x.TransactionDate);
                x.Summary = TrimContent(x.Summary);
                x.Description = TrimContent(x.Description);
                x.Region = TrimContent(x.Region);
                x.Unit = TrimContent(x.Unit);
                x.TransactionIncome = TrimContent(x.TransactionIncome);
                x.TransactionOutCome = TrimContent(x.TransactionOutCome);
                x.TransactionCurrency = TrimContent(x.TransactionCurrency);
                x.Income = TrimContent(x.Income);
                x.Outcome = TrimContent(x.Outcome);
                x.Currency = TrimContent(x.Currency);
                x.AccountingCurrencyBalance = TrimContent(x.AccountingCurrencyBalance);
                x.TransactionAccount = TrimContent(x.TransactionAccount);
            });
            dataList = dataList.Where(x => !string.IsNullOrWhiteSpace(x.TransactionDate)).ToList();

            #endregion

            var groupDataList = dataList.GroupBy(x => !string.IsNullOrEmpty(x.Income))
                .ToLookup(g => g.Key, g => g.ToList());

            var exportTemplate = new ExportSuiBill();

            foreach (var group in groupDataList)
            {
                // 收入
                if (group.Key)
                {
                    foreach (var items in group)
                    {
                        exportTemplate.Income = items.Select(x => new SuiTemplateBill
                        {
                            TransactionDateTime = DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            Category = "职业收入",
                            SubCategory = "利息收入",
                            SourceAccount = "中国工商银行",
                            Amount = decimal.Parse(TrimContent(x.Income)),
                            Remark = x.Description,
                        }).ToList();
                    }
                }

                // 支出或转帐
                else
                {
                    foreach (var items in group)
                    {
                        // 转帐: 找出支付宝或微信支付纪录
                        exportTemplate.Transfer = items
                            .Where(x => x.TransactionAccount.Contains("支付宝") || x.TransactionAccount.Contains("财付通"))
                            .Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                                    SourceAccount = "中国工商银行",
                                    TargetAccount = SuiTemplateBillHelper.GetTargetAccount(x.TransactionAccount),
                                    Amount = decimal.Parse(x.Outcome)
                                }).ToList();

                        // 支出
                        exportTemplate.Outgo = items
                            .Where(x => !x.TransactionAccount.Contains("支付宝") && !x.TransactionAccount.Contains("财付通"))
                            .Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        DateTime.Parse(x.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss"),
                                    Category = "其他杂项",
                                    SubCategory = "其他支出",
                                    SourceAccount = "中国工商银行",
                                    Amount = decimal.Parse(x.Outcome),
                                    Remark = x.Description,
                                }).ToList();
                    }
                }
            }

            return exportTemplate;
        }

        private string TrimContent(string content)
        {
            return content.Replace("\"", "").TrimEnd('\t').Trim();
        }
    }
}