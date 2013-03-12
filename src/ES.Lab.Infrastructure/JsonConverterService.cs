using System;
using Newtonsoft.Json;
using Treefort;
namespace ES.Lab.Infrastructure
{
    public class JsonConverterService : IJsonConverter
    {
        public object DeserializeObject(string json, System.Type type)
        {
            var value = JsonConvert.DeserializeObject(json, type);
            return value;
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    public class GuidConverter : JsonConverter
    {

        public override bool CanConvert(System.Type objectType)
        {
            return typeof(Guid) == objectType;
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return Guid.Empty;
                case JsonToken.String:
                    var str = reader.Value as string;
                    if (string.IsNullOrEmpty(str)) return Guid.Empty;
                    else
                    {
                        var id = Guid.Parse(reader.Value.ToString());
                        return id;
                    }
                default:
                    throw new ArgumentException("Invalid token type");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Guid.Empty.Equals(value))
            {
                writer.WriteValue("");
            }
            else
            {
                writer.WriteValue((Guid)value);
            }
        }
    }
}