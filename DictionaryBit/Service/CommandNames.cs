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
        public const string SetActiveDictionary = "/setactivedictionary";
        public const string LossActiveDictionary = "/lossactivedictionary";
    }
}
