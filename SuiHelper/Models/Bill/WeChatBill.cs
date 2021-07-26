namespace SuiHelper.Models.Bill
{
    /// <summary>
    /// 微信帐单
    /// </summary>
    public class WeChatBill
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 交易对方
        /// </summary>
        public string Counterparty { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public string Merchandise { get; set; }

        /// <summary>
        /// 收/支
        /// </summary>
        public string TradingBehavior { get; set; }

        /// <summary>
        /// 金额(元)
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayMethod { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// 交易单号
        /// </summary>
        public string TransactionNo { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string MerchantNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}