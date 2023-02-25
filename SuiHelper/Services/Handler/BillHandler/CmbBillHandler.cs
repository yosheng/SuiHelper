using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SuiHelper.Helper;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class CmbBillHandler : AbsBillHandler
    {
        public override ExportSuiBill GetExportSuiBill(string uploadFilePath)
        {
            var configuration = new CsvConfiguration(CultureInfo.CurrentUICulture)
            {
                TrimOptions = TrimOptions.Trim,
                HasHeaderRecord = true,
                ShouldSkipRecord = recordArgs =>
                {
                    if (recordArgs.Row[0] == null) return false;
                    return recordArgs.Row[0].StartsWith("#");
                }
            };

            using var reader = new StreamReader(uploadFilePath, Encoding.GetEncoding("GB2312"));
            using var csv = new CsvReader(reader, configuration);
            csv.Read();
            csv.ReadHeader();
            var records = new List<CmbBill>();
            
            while (csv.Parser.Read())
            {
                if (csv.Parser.Record == null)
                {
                    continue;
                }

                if (csv.Parser.Record[0].StartsWith("#"))
                {
                    continue;
                }

                records.Add(csv.GetRecord<CmbBill>());
            }
            
            #region 处理元数据

            records.ForEach(x =>
            {
                x.TransactionDate = TrimContent(x.TransactionDate);
                x.TransactionTime = TrimContent(x.TransactionTime);
                x.AccountingCurrencyBalance = TrimContent(x.AccountingCurrencyBalance);
                x.TransactionType = TrimContent(x.TransactionType);
                x.TransactionRemark = TrimContent(x.TransactionRemark);
            });

            #endregion
            
            var groupDataList = records.GroupBy(x => x.Outcome == 0)
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
                            .Where(x => x.TransactionRemark.Contains("支付宝") || x.TransactionRemark.Contains("财付通")).Select(x =>
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
                            .Where(x => !x.TransactionRemark.Contains("支付宝") && !x.TransactionRemark.Contains("财付通")).Select(x =>
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