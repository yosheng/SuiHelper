using System;
using System.Linq;
using Ganss.Excel;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class CmbBillHandler : AbsBillHandler
    {
        public override ExportSuiBill GetExportSuiBill(string uploadFilePath)
        {
            var excelFile = new ExcelMapper(uploadFilePath)
            {
                HeaderRowNumber = 7,
                MinRowNumber = 7
            };
            excelFile.AddMapping<CmbBill>("交易日期", d => d.TransactionDate);
            excelFile.AddMapping<CmbBill>("交易时间", d => d.TransactionTime);
            excelFile.AddMapping<CmbBill>("收入", d => d.Income);
            excelFile.AddMapping<CmbBill>("支出", d => d.Outcome);
            excelFile.AddMapping<CmbBill>("余额", d => d.AccountingCurrencyBalance);
            excelFile.AddMapping<CmbBill>("交易类型", d => d.TransactionType);
            excelFile.AddMapping<CmbBill>("交易备注", d => d.TransactionRemark);
            var dataList = excelFile.Fetch<CmbBill>().ToList();
            
            #region 处理元数据

            dataList.ForEach(x =>
            {
                x.TransactionDate = TrimContent(x.TransactionDate);
                x.TransactionTime = TrimContent(x.TransactionTime);
                x.AccountingCurrencyBalance = TrimContent(x.AccountingCurrencyBalance);
                x.TransactionType = TrimContent(x.TransactionType);
                x.TransactionRemark = TrimContent(x.TransactionRemark);
            });

            #endregion
            
            var groupDataList = dataList.GroupBy(x => x.Outcome == 0)
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
                            SourceAccount = "招商银行",
                            Amount = x.Income,
                            Remark = x.TransactionRemark,
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
                            .Where(x => x.TransactionRemark == "支付宝" || x.TransactionRemark == "财付通").Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        GetTransactionDateTime(x.TransactionDate, x.TransactionTime),
                                    SourceAccount = "招商银行",
                                    TargetAccount = SuiTemplateBillHelper.GetTargetAccount(x.TransactionRemark),
                                    Amount = x.Outcome,
                                }).ToList();

                        // 支出
                        exportTemplate.Outgo = item
                            .Where(x => x.TransactionRemark != "支付宝" && x.TransactionRemark != "财付通").Select(x =>
                                new SuiTemplateBill
                                {
                                    TransactionDateTime =
                                        GetTransactionDateTime(x.TransactionDate, x.TransactionTime),
                                    Category = "其他杂项",
                                    SubCategory = "其他支出",
                                    SourceAccount = "招商银行",
                                    Amount = x.Outcome,
                                    Remark = x.TransactionRemark,
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
                return $"{transactionDate.Substring(0, 4)}-{transactionDate.Substring(4, 2)}-{transactionDate.Substring(6, 2)} {transactionTime}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"日期:{transactionDate} 时间:{transactionTime} 异常{e}");
                throw;
            }
        }
        
        private string TrimContent(string content)
        {
            return content.Replace("\"", "").TrimEnd('\t').Trim();
        }
    }
}