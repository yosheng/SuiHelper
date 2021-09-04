using System.Text;

namespace SuiHelper.Services.Manager
{
    public interface IBillManager
    {
        string GetExcelPathByCsvPath(string uploadFilePath, Encoding encoding);
    }
}