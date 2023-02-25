using CsvHelper.Configuration.Attributes;

namespace SuiHelper.Models.Bill
{
    /// <summary>
    /// 招商银行帐单
    /// </summary>
    [Delimiter(",")]
    public class CmbBill
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [Name("交易日期")] 
        public string TransactionDate { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        [Name("交易时间")] 
        public string TransactionTime { get; set; }
        
        /// <summary>
        /// 收入
        /// </summary>
        [Name("收入")] 
        [Default(0)]
        public decimal Income { get; set; }

        /// <summary>
        /// 支出
        /// </summary>
        [Name("支出")] 
        [Default(0)]
        public decimal Outcome { get; set; }
        
        /// <summary>
        /// 余额
        /// </summary>
        [Name("余额")] 
        public string AccountingCurrencyBalance { get; set; }
        
        /// <summary>
        /// 交易类型
        /// </summary>
        [Name("交易类型")] 
        public string TransactionType { get; set; }
        
        /// <summary>
        /// 交易备注
        /// </summary>
        [Name("交易备注")] 
        public string TransactionRemark { get; set; }
    }
}