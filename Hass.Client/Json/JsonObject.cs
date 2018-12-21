using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Hass.Client.Json
{
    public sealed class JsonObject
    {
        private Dictionary<string, object> jsonData;

        public JsonObject()
            : this(new Dictionary<string, object>())
        {

        }

        private JsonObject(Dictionary<string, object> jsonData)
        {
            this.jsonData = jsonData;
        }

        public static JsonObject Parse(string json)
        {
            Dictionary<string, object> jsonData =
                 new JsonSerializer().Deserialize<Dictionary<string, object>>(
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

        public object this[string key]
        {
            get
            {
                return jsonData[key];
            }
            set
            {
                var jsonObj = value as JsonObject;
                if(jsonObj != null)
                {
                    value = jsonObj.jsonData;
                }
                jsonData[key] = value;
            }
        }

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            object val = null;
            if (jsonData == null || !jsonData.TryGetValue(key, out val) || val == null)
            {
                return defaultValue;
            }

            Type tp = typeof(T);
            if (tp == typeof(string))
            {
                return (T)(object)val.ToString();
            }
            if(tp == typeof(JsonObject))
            {
                return (T)(object)new JsonObject((Dictionary<string, object>)val);
            }
            if (tp == typeof(JsonObject[]))
            {
                 
            }

            return (T)val;
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
