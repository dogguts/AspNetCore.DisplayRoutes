using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Report.Json {
    public class ShortTypeNameJsonConverter : JsonConverter {
        private readonly Type[] _types;

        public ShortTypeNameJsonConverter(params Type[] types) {
            _types = types;
        }

        public override bool CanConvert(Type objectType) {
            return _types.Any(t => t == objectType || t.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            string stringValue;

            if (typeof(Type).IsAssignableFrom(value.GetType())) {
                stringValue = ((Type)value).FullName;
            } else {
                stringValue = value.ToString();
            }

            var t = Newtonsoft.Json.Linq.JToken.FromObject(stringValue);
            t.WriteTo(writer);
        }
    }
}
