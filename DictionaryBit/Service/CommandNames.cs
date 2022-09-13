using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Service
{
    public static class CommandNames
    {
        public const string DictionaryNameData = "/dictionaryname";
        public const string Start = "/start";
        public const string AddDictionary = "/adddictionary";
        public const string AddDictionaryName = "/adddictionary name";
        public const string SlashWarning = "/slashwarning";
        public const string GetDictionaries = "/getdictionaries";
        public const string GetWords = "/getwords";
        public const string AddEditWordForeign = "/addword foreign";
        public const string AddEditWordNative = "/addword native";
        public const string AddEditWordDescription = "/addword description";
        public const string Test = "/test";
        public const string Default = "/default";
        public const string UseDicrionary = "/usedictionary";
        public const string SetUsedDictionary = "/setuseddictionary";
        public const string RemoveUsedDictionary = "/removeuseddictionary";
        public const string SaveWord = "/saveword";
        public const string AddWord = "/addword";
        public const string EditWord = "/editword";
        public const string DeleteWord = "/deleteword";
    }
}
