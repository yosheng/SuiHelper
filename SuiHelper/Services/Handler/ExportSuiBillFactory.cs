using System;
using SuiHelper.Common;
using SuiHelper.Services.Handler.BillHandler;

namespace SuiHelper.Services.Handler
{
    public static class ExportSuiBillFactory
    {
        public static AbsBillHandler CreateBillHandler(BillType billType)
        {
            AbsBillHandler billHandler;
            switch (billType)
            {
                case BillType.AbChina:
                    billHandler = new AbChinaBillHandler();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(billType), billType, null);
            }

            return billHandler;
        }
    }
}