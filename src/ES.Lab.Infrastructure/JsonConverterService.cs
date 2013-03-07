using Newtonsoft.Json;
using Treefort;
namespace ES.Lab.Infrastructure
{
    public class JsonConverterService : IJsonConverter
    {
        public object DeserializeObject(string json, System.Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}