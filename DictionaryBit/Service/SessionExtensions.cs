using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Service
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var jsonedValue = JsonConvert.SerializeObject(value);
            session?.SetString(key, jsonedValue);
        }
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session?.GetString(key) ?? string.Empty;
            try
            {
                var deserializedValue = JsonConvert.DeserializeObject<T>(value);
                return deserializedValue;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
