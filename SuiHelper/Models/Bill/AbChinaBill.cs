namespace SuiHelper.Models.Bill
{
    public class AbChinaBill
    {
        public string TransactionDate { get; set; }
	
        public string TransactionTime { get; set; }
	
        public decimal TransactionAmount { get; set; }
	
        public decimal CurrentBalance { get; set; }
	
        public string TransactionName { get; set; }
	
        public string TransactionAccount { get; set; }
	
        public string TransactionBranch { get; set; }

        public string TransactionChannel { get; set; }
	
        public string TransactionType { get; set; }

        public string TransactionPurpose { get; set; }

        public string TradingSummary { get; set; }
    }
}