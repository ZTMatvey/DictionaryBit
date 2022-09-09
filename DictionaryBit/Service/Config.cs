using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Service
{
    public sealed class Config
    {
        private static Config _instance = new Config();
        public static Config Instance=> _instance;
        public string Token { get; set; }
        public string Url { get; set; }
        public static void UpdateInstance(Config instance)
        {
            _instance = instance;
        }
    }
}
