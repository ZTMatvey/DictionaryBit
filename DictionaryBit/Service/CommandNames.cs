using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Service
{
    public static class CommandNames
    {
        public const string CurrentOperation = "CurrentOperation";
        public const string Start = "/start";
        public const string AddDictionary = "/addDictionary";
        public const string AddDictionaryName = "/addDictionary name";
        public const string SlashWarning = "/slashWarning";
        public const string GetDictionaries = "/getDictionaries";
        public const string AddWord = "/addWord";
        public const string AddWordForeign = "/addWord foreign";
        public const string AddWordNative = "/addWord native";
        public const string AddWordDescription = "/addWord description";
        public const string Test = "/test";
        public const string Default = "/default";
        public const string UseDicrionary = "/useDictionary";
        public const string SetUsedDictionary = "/setUsedDictionary";
        public const string RemoveUsedDictionary = "/removeUsedDictionary";
        public const string SaveWord = "/saveWord";
    }
}
