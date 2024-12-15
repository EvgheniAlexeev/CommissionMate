using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Models.JsonConvertor
{
    public class CamelPascalCaseConverter<T> : JsonConverter<T> where T : class
    {
        private readonly CamelPascalCaseNamingPolicy _namingPolicy = new();
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token.");
            }

            var instance = Activator.CreateInstance(typeToConvert);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return instance as T;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected PropertyName token.");
                }

                string jsonPropertyName = reader.GetString();
                var property = typeToConvert.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => _namingPolicy.IsValidName(jsonPropertyName, p.Name));

                if (property == null)
                {
                    throw new JsonException($"Property '{jsonPropertyName}' not found or not in camelCase/PascalCase format.");
                }

                reader.Read(); // Move to the value
                var value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                property.SetValue(instance, value);
            }

            throw new JsonException("Unexpected end of JSON.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Write is not implemented.");
        }
    }
}
