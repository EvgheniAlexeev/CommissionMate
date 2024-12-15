using System.Text.Json;

namespace Domain.Models.JsonConvertor
{
    public class CamelPascalCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        }

        public bool IsValidName(string propertyName, string modelPropertyName)
        {
            return propertyName.Equals(modelPropertyName, StringComparison.Ordinal) ||
                   propertyName.Equals(Char.ToLowerInvariant(modelPropertyName[0]) + modelPropertyName.Substring(1), StringComparison.Ordinal);
        }
    }
}
