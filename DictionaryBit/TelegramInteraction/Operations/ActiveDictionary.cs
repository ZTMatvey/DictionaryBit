using DictionaryBit.Data.Entities;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.TelegramInteraction.Operations
{
    public sealed class ActiveDictionary
    {
        public Dictionary? Get(ISession session)
        {
            return session.Get<Dictionary>(SessionKeyNames.ActiveDictionary);
        }
        public void Set(ISession session, Dictionary dictionary)
        {
            session.Set(SessionKeyNames.ActiveDictionary, dictionary);
        }
        public void Loss(ISession session)
        {
            session.Remove(SessionKeyNames.ActiveDictionary);
        }
    }
}
