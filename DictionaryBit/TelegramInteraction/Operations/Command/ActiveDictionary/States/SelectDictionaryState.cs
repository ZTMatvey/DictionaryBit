using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary.States
{
    public class SelectDictionaryState : IActiveDictionaryState
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly User _user;
        private readonly TelegramBotClient _botClient;
        public SelectDictionaryState(RepositoryManager repositoryManager, User user, TelegramBotClient botClient)
        {
            _repositoryManager = repositoryManager;
            _user = user;
            _botClient = botClient;
        }
        public async Task ExecuteAsync()
        {
            var dictionaryKeyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, _user);
            await _botClient.SendTextMessageAsync(_user.ChatId, "Выберите словарь для активации", replyMarkup: dictionaryKeyboard);
        }
    }
}
