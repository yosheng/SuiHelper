namespace SuiHelper.Models.Bill
{
    /// <summary>
    /// 招商银行帐单
    /// </summary>
    public class CmbBill
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TransactionTime { get; set; }
        
        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 支出
        /// </summary>
        public decimal Outcome { get; set; }
        
        /// <summary>
        /// 余额
        /// </summary>
        public string AccountingCurrencyBalance { get; set; }
        
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType { get; set; }
        
        /// <summary>
        /// 交易备注
        /// </summary>
        public string TransactionRemark { get; set; }
    }
}