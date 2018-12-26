using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hass.Client.Json
{
    public sealed class JsonObject
    {
        private Dictionary<string, JToken> jsonData;

        public JsonObject()
            : this(new Dictionary<string, JToken>())
        {

        }

        private JsonObject(Dictionary<string, JToken> jsonData)
        {
            this.jsonData = jsonData;
        }

        public static JsonObject Parse(string json)
        {
            JValue v;
            
            Dictionary<string, JToken> jsonData =
                 new JsonSerializer().Deserialize<Dictionary<string, JToken>>(
                            new JsonTextReader(new StringReader(json)));

            return new JsonObject(jsonData);
        }

        public static JsonObject Create(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            string json = obj is string ? (string)obj : Stringify(obj);
            return Parse(json);
        }


        private object JTokenToValue(JToken token, Type type)
        {
            Action<Type> checkValueType = (valTp) =>
            {
                if (type != valTp)
                {
                    throw new ArgumentException($"expect typeof(T) is {valTp}");
                }
            };



            JObject obj = token as JObject;
            if (obj != null)
            {
                checkValueType(typeof(JsonObject));
                return new JsonObject(((IEnumerable<KeyValuePair<string, JToken>>)obj).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            }

            JArray array = token as JArray;
            if (array != null)
            {
                array.Select(t => (JsonObject)JTokenToValue(t, type.GetElementType())).ToArray();
            }
            return token.ToObject(type);
        }


        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            JToken tok = null;
            if (jsonData == null || !jsonData.TryGetValue(key, out tok) || tok == null)
            {
                return defaultValue;
            }
            return (T)(object)JTokenToValue(tok, typeof(T));
        }

        public void SetValue<T>(string key, T value)
        {
            jsonData[key] = new JValue(value);
        }

        public string Stringify()
        {
            return Stringify(this);
        }

        public static string Stringify(object obj)
        {
            if(obj == null)
            {
                return null;
            }
            JsonObject jsonObj = obj as JsonObject;
            if(jsonObj != null)
            {
                obj = jsonObj.jsonData;
            }

            var writer = new StringWriter();
            new JsonSerializer().Serialize(writer, obj);
            writer.Flush();
            return writer.ToString();
        }

    }
}
