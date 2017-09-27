using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Common.Helpers
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var enumField = value.GetType().GetField(value.ToString());
            var customAttributes = (IList<DescriptionAttribute>)enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (customAttributes.Count > 0)
            {
                return customAttributes[0].Description;
            }

            return value.ToString();
        }
    }
}
