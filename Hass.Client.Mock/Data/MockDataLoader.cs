using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Hass.Client.Mock.Data
{

    public class MockDataLoader
    {

        public static string LoadData<T>(string fileName)
        {
            string path = $"{typeof(MockDataLoader).Namespace}.{typeof(T).FullName}.{fileName}";
            using (Stream st = typeof(MockDataLoader).Assembly.GetManifestResourceStream(path))
            {
                return new StreamReader(st).ReadToEnd();
            }
        }

        public static JObject LoadJson<T>(string fileName)
        {
            string json = LoadData<T>(fileName);
            return JObject.Parse(json);
        }

    }

}
