namespace SuiHelper.Models
{
    /// <summary>
    /// 随手记帐单
    /// </summary>
    public class SuiTemplateBill
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string TransactionDateTime { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 子分类
        /// </summary>
        public string SubCategory { get; set; }

        /// <summary>
        /// 帐户1
        /// </summary>
        public string SourceAccount { get; set; }

        /// <summary>
        /// 帐户2
        /// </summary>
        public string TargetAccount { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 成员
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// 商家
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}