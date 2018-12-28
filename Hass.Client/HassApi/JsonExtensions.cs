using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Hass.Client.HassApi
{
    public static class JsonExtensions
    {

        public static T GetPathedValue<T>(this JObject obj, string path)
        {
            string[] keys = path.Split(new char[] {'.'});
            JObject val = obj;
            for(int i = 0; i < keys.Length-1; i++)
            {
                val = val.GetValue<JObject>(keys[i]);
            }
            return GetValue<T>(val, keys[keys.Length-1]);
        }

        public static T GetValue<T>(this JObject obj, string key)
        {
            if(obj == null)
            {
                throw new ArgumentNullException("this obj");
            }

            JToken val = null;
            if (obj == null || !obj.TryGetValue(key, StringComparison.InvariantCultureIgnoreCase, out val) || val.Type == JTokenType.Null)
            {
                return default(T);
            }

            if (val.Type == JTokenType.String)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)DateTime.Parse(val.Value<string>());
                }
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)val.Value<string>();
                }
                throw new ArgumentException($"cannot convert json string to: {typeof(T)}");
            }
            if (val.Type == JTokenType.Integer 
                || val.Type == JTokenType.Float
                || val.Type == JTokenType.Boolean
                || val.Type == JTokenType.Date
                || val.Type == JTokenType.String)
            {
                return (T)val.Value<T>();
            }
            if(val.Type == JTokenType.Array)
            {
                return (T)(object)val.Values<JObject>().ToArray();
            }
            return (T)(object)val.Value<JObject>();
        }
    }

}
