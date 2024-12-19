using System.ComponentModel;

namespace Domain.Extensions
{
    public static class EnumExtension
    {
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

        public static Dictionary<int, string> EnumToDictionary<T>() where T : Enum
        {
            return Enum
                .GetValues(typeof(T)).Cast<T>()
                .ToDictionary(x => Convert.ToInt32(x), x => x.ToDescription());
        }
    }
}
