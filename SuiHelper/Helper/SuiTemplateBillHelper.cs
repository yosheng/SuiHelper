namespace SuiHelper.Helper
{
    public class SuiTemplateBillHelper
    {
        public static string GetTargetAccount(string tradingSummary)
        {
            return tradingSummary switch
            {
                { } a when a.Contains("财付通") => "微信钱包",
                { } a when a.Contains("支付宝") => "支付宝",
                _ => ""
            };
        }
    }
}