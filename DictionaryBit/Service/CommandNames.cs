using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Service
{
    public static class CommandNames
    {
        public const string DictionaryIdData = "/dictionaryid";
        public const string Start = "/start";
        public const string SymbolWarning = "/symbolwarning";
        public const string Default = "/default";
        public const string UseDictionary = "/usedictionary";
        public const string LossActiveDictionary = "/lossdictionary";
        public const string AddDictionary = "/adddictionary";
        public const string RemoveDictionary = "/removedictionary";
        public const string GetDictionaries = "/getdictionaries";
        public const string GetWords = "/getwords";
        public const string EditWord = "/editwords";
        public const string RemoveWord = "/removewords";
        public const string AddWord = "/addword";
    }
}
