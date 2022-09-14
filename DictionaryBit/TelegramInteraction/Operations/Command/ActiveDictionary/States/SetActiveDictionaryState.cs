using DictionaryBit.Data.Entities;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary.States
{
    public class SetActiveDictionaryState : IState
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISession _session;
        private readonly User _user;
        private readonly Dictionary _dictionary;
        private readonly ActiveDictionary _activeDictionary;

        public SetActiveDictionaryState(TelegramBotClient botClient, ISession session, User user, Dictionary dictionary, ActiveDictionary activeDictionary)
        {
            _botClient = botClient;
            _session = session;
            _user = user;
            _dictionary = dictionary;
            _activeDictionary = activeDictionary;
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
