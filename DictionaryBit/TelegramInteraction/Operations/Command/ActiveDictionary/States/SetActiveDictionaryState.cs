using DictionaryBit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary.States
{
    public class SetActiveDictionaryState : IActiveDictionaryState
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISession _session;
        private readonly User _user;
        private readonly Dictionary _dictionary;
        private readonly Operations.ActiveDictionary _activeDictionary;
        private readonly string _content;

        public SetActiveDictionaryState(TelegramBotClient botClient, ISession session, User user, Dictionary dictionary, Operations.ActiveDictionary activeDictionary, string content)
        {
            this._botClient = botClient;
            this._session = session;
            this._user = user;
            this._dictionary = dictionary;
            this._activeDictionary = activeDictionary;
            this._content = content;
        }
        public async Task ExecuteAsync()
        {
            if (_dictionary == default)
                await _botClient.SendTextMessageAsync(_user.ChatId, $"У вас нет данного словаря");
            else
            {
                _activeDictionary.Set(_session, _dictionary);
                await _botClient.SendTextMessageAsync(_user.ChatId, $"Словарь '{_dictionary.Name}' теперь активен");
            }
        }
    }
}
