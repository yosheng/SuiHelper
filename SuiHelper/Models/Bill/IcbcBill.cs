namespace SuiHelper.Models.Bill
{
    /// <summary>
    /// 工商银行帐单
    /// </summary>
    public class IcbcBill
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 交易场所
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 交易国家或地区简称
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 钞/汇
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 交易金额(收入)
        /// </summary>
        public string TransactionIncome { get; set; }

        /// <summary>
        /// 交易金额(支出)
        /// </summary>
        public string TransactionOutCome { get; set; }
        
        /// <summary>
        /// 交易币种
        /// </summary>
        public string TransactionCurrency { get; set; }

        /// <summary>
        /// 记账金额(收入)
        /// </summary>
        public string Income { get; set; }

        /// <summary>
        /// 记账金额(支出)
        /// </summary>
        public string Outcome { get; set; }

        /// <summary>
        /// 记账币种
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public string AccountingCurrencyBalance { get; set; }
        
        /// <summary>
        /// 对方户名
        /// </summary>
        public string TransactionAccount { get; set; }
    }
}