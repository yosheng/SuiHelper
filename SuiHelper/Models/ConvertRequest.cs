using Microsoft.AspNetCore.Http;
using SuiHelper.Common;

namespace SuiHelper.Models
{
    public class ConvertRequest
    {
        public IFormFile File { get; set; }

        public BillType BillType { get; set; }
    }
}