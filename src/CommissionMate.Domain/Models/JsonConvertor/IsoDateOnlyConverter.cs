using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Models.JsonConvertor
{
    public class IsoDateOnlyConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize the date string back into a DateTime
            return DateTime.ParseExact(reader.GetString()!, DateFormat, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Serialize the DateTime to a string in "yyyy-MM-dd" format
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
}
