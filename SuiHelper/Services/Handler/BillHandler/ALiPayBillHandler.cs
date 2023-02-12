using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SuiHelper.Models;
using SuiHelper.Models.Bill;

namespace SuiHelper.Services.Handler.BillHandler
{
    public class ALiPayBillHandler : AbsBillHandler
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
                    return recordArgs.Row[0].StartsWith("支付宝交易记录明细查询") || recordArgs.Row[0].StartsWith("账号:") ||
                           recordArgs.Row[0].StartsWith("起始日期:") ||
                           recordArgs.Row[0].StartsWith("---------------------------------");
                }
            };

            using var reader = new StreamReader(uploadFilePath, Encoding.GetEncoding("GB2312"));
            using var csv = new CsvReader(reader, configuration);
            csv.Read();
            csv.ReadHeader();
            var records = new List<ALiPayBill>();
            
            while (csv.Parser.Read())
            {
                if (csv.Parser.Record == null)
                {
                    continue;
                }

                if (csv.Parser.Record[0].StartsWith("---------------------------------") ||
                    csv.Parser.Record[0].Contains("笔记录") || csv.Parser.Record[0].StartsWith("已收入:") ||
                    csv.Parser.Record[0].StartsWith("待收入:") || csv.Parser.Record[0].StartsWith("已支出:") ||
                    csv.Parser.Record[0].StartsWith("待支出:") || csv.Parser.Record[0].StartsWith("导出时间:"))
                {
                    continue;
                }

                records.Add(csv.GetRecord<ALiPayBill>());
            }

            var groupDataList = records.GroupBy(x => x.TransactionType)
                .ToLookup(g => g.Key, g => g.ToList());
            
            var exportTemplate = new ExportSuiBill();
            
            foreach (var group in groupDataList)
            {
                // 收入
                if (group.Key.Equals("收入"))
                {
                    foreach (var item in group)
                    {
                        exportTemplate.Income.AddRange(item.Select(x => new SuiTemplateBill
                        {
                            TransactionDateTime = GetTransactionDateTime(x.TransactionTime),
                            Category = "其他收入",
                            SubCategory = "经营所得",
                            SourceAccount = "支付宝",
                            Amount = decimal.Parse(x.Amount),
                            Remark = GetRemark(x.TransactionTarget, x.Name),
                        }).ToList());
                    }
                }                
                if (group.Key.Equals("其他"))
                {
                    foreach (var item in group)
                    {
                        exportTemplate.Income.AddRange(item.Select(x => new SuiTemplateBill
                        {
                            TransactionDateTime = GetTransactionDateTime(x.TransactionTime),
                            Category = "职业收入",
                            SubCategory = "投资收入",
                            SourceAccount = "支付宝",
                            Amount = decimal.Parse(x.Amount),
                            Remark = GetRemark(x.TransactionTarget, x.Name),
                        }).ToList());
                    }
                }
                // 支出
                if (group.Key.Equals("支出"))
                {
                    foreach (var item in group)
                    {
                        // 支出
                        exportTemplate.Outgo = item.Select(x =>
                            new SuiTemplateBill
                            {
                                TransactionDateTime =
                                    GetTransactionDateTime(x.TransactionTime),
                                Category = "其他杂项",
                                SubCategory = "其他支出",
                                SourceAccount = "支付宝",
                                Amount = decimal.Parse(x.Amount),
                                Remark = GetRemark(x.TransactionTarget, x.Name),
                            }).ToList();
                    }
                }
            }
            
            return exportTemplate;
        }

        private string GetRemark(string transactionTarget, string name)
        {
            return $"{transactionTarget}-{name}";
        }
        
        private string GetTransactionDateTime(string transactionTime)
        {
            try
            {
                var time = DateTime.Parse(transactionTime);
                return time.ToString("yyyy-MM-dd hh:mm:ss");
            }
            catch (Exception e)
            {
                Console.WriteLine($"交易时间:{transactionTime} 异常{e}");
                throw;
            }
        }
    }
}