using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bookstore.API.Converters
{
    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);

            if (jsonDoc.RootElement.ValueKind == JsonValueKind.Object)
            {
                var text = jsonDoc.RootElement.GetRawText();
                var date = JsonSerializer.Deserialize<Date>(text);

                if (date!.DateIsValid())
                {
                    return date.GetDate();
                }
            }

            return DateOnly.Parse(jsonDoc.RootElement.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            var isoDate = value.ToString();
            writer.WriteStringValue(isoDate);
        }
    }

    [Serializable]
    public class Date
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }

        public bool DateIsValid()
        {
            return Year != 0 && Month != 0 && Day != 0;
        }

        public DateOnly GetDate()
        {
            return new DateOnly(Year, Month, Day);
        }
    }
}
