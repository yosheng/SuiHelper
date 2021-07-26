using System.Collections.Generic;

namespace SuiHelper.Models
{
    public class ExportSuiBill
    {
        public ExportSuiBill()
        {
            Income = new List<SuiTemplateBill>();
            Outgo = new List<SuiTemplateBill>();
            Transfer = new List<SuiTemplateBill>();
        }
        
        public List<SuiTemplateBill> Income { get; set; }

        public List<SuiTemplateBill> Outgo { get; set; }

        public List<SuiTemplateBill> Transfer { get; set; }
    }
}