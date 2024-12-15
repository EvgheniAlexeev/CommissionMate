using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class EnumExtension
    {
        public static string? ConvertToString<T>(this T ev) where T : Enum
        {
            return Enum.GetName(typeof(T), ev);
        }

        public static T ConverToEnum<T>(this string enumValue) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), enumValue);
        }

        public static string ToDescription<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return enumValue.ToString();
        }
    }
}
