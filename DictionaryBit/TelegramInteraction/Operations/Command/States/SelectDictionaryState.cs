using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.States
{
    public class SelectDictionaryState : IState
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly User _user;
        private readonly TelegramBotClient _botClient;
        private readonly string _dictionariesFor;
        public SelectDictionaryState(RepositoryManager repositoryManager, User user, TelegramBotClient botClient, string dictionariesFor = "")
        {
            _repositoryManager = repositoryManager;
            _user = user;
            _botClient = botClient;
            _dictionariesFor = dictionariesFor;
        }
        public async Task ExecuteAsync()
        {
            var dictionaryKeyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, _user);
            await _botClient.SendTextMessageAsync(_user.ChatId, $"Выберите словарь {_dictionariesFor}", replyMarkup: dictionaryKeyboard);
        }
    }
}
