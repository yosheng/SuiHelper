using CsvHelper.Configuration.Attributes;

namespace SuiHelper.Models.Bill
{
    [Delimiter(",")]
    public class ALiPayBill
    {
        [Name("交易号")] public string TransactionNo { get; set; }

        [Name("商家订单号")] public string OrderNo { get; set; }

        [Name("交易创建时间")] public string TransactionTime { get; set; }

        [Name("付款时间")] public string PayTime { get; set; }

        [Name("最近修改时间")] public string ModifyTime { get; set; }

        [Name("交易来源地")] public string TransactionSource { get; set; }

        [Name("类型")] public string Type { get; set; }

        [Name("交易对方")] public string TransactionTarget { get; set; }

        [Name("商品名称")] public string Name { get; set; }

        [Name("金额（元）")] public string Amount { get; set; }

        [Name("收/支")] public string TransactionType { get; set; }
        
        [Name("交易状态")] public string TransactionStatus { get; set; }
        
        [Name("服务费（元）")] public string Service { get; set; }
        
        [Name("成功退款（元）")] public string RefundAmount { get; set; }
        
        [Name("备注")] public string Remark { get; set; }
        
        [Name("资金状态")] public string AmountStatus { get; set; }
    }
}