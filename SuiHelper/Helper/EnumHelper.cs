using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SuiHelper.Common;

namespace SuiHelper.Helper
{
    public class EnumHelper
    {
        public static Dictionary<int, Tuple<string, string, string>> GetEnumItem(Type type)
        {
            var result = new Dictionary<int, Tuple<string, string, string>>();

            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                if (fieldInfo.FieldType.IsEnum)
                {
                    var displayAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                    var validFileTypeAttributes = fieldInfo.GetCustomAttributes(typeof(ValidFileTypeAttribute), false) as ValidFileTypeAttribute[];
                    if (displayAttributes == null || validFileTypeAttributes == null)
                    {
                        continue;
                    }
                    if (displayAttributes.Length > 0 && validFileTypeAttributes.Length > 0)
                    {
                        var strValue = ((int)type.InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, null, null));
                        var data = Tuple.Create(displayAttributes[0].Name, displayAttributes[0].Description, validFileTypeAttributes[0].FileType);
                        result.Add(strValue, data);
                    }
                }
            }

            return result;
        }
    }
}