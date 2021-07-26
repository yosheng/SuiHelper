using SuiHelper.Models;

namespace SuiHelper.Services.Handler
{
    public abstract class AbsBillHandler
    {
        public abstract ExportSuiBill GetExportSuiBill(string uploadFilePath);
    }
}