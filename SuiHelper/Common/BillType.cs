using System;
using System.ComponentModel.DataAnnotations;

namespace SuiHelper.Common
{
    public enum BillType
    {
        /// <summary>
        /// 微信帐单
        /// </summary>
        [Display(Name = "微信", Description = "请到微信钱包中右上角选择帐单并点选下载帐单解压缩后格式为csv")]
        [ValidFileType("csv")]
        WeChat = 1,
        
        /// <summary>
        /// 中国农业银行
        /// </summary>
        [Display(Name = "中国农业银行", Description = "登入农业银行网页中导出帐务明细格式为xls")]
        [ValidFileType("xls")]
        AbChina = 2,
        
        /// <summary>
        /// 中国农业银行
        /// </summary>
        [Display(Name = "中国工商银行", Description = "登入工商银行网页中导出帐务明细格式为csv")]
        [ValidFileType("csv")]
        Icbc = 3,
        
        /// <summary>
        /// 中国农业银行
        /// </summary>
        [Display(Name = "招商银行", Description = "登入招商银行网页中导出帐务明细格式为csv")]
        [ValidFileType("csv")]
        Cmb = 4
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidFileTypeAttribute : Attribute
    {
        private string _fileType;

        public string FileType
        {
            get => _fileType;
            set => _fileType = value;
        }

        public ValidFileTypeAttribute(string fileType)
        {
            _fileType = fileType;
        }
    }
}