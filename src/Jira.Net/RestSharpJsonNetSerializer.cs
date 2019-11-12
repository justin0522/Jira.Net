using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Jira.Net.Models
{
    public class RestSharpJsonNetSerializer : ISerializer, IDeserializer
    {
        private JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public string Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
            return json;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, settings);
        }

        public string ContentType { get; set; } = "application/json";
    }
}
