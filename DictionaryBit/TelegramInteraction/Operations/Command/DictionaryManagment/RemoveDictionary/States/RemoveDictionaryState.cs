using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.RemoveDictionary.States
{
    public class RemoveDictionaryState : IState
    {
        private readonly TelegramBotClient _botClient;
        private readonly RepositoryManager _repositoryManager;
        private readonly Dictionary _dictionary;
        private readonly User _user;
        public RemoveDictionaryState(RepositoryManager repositoryManager, TelegramBotClient botClient, User user, Dictionary dictionary)
        {
            _repositoryManager = repositoryManager;
            _dictionary = dictionary;
            _user = user;
            _botClient = botClient;
        }
        public async Task ExecuteAsync()
        {
            if (_dictionary == null || _dictionary.UserId != _user.Id)
            {
                await _botClient.SendTextMessageAsync(_user.ChatId, "У вас нет данного словаря");
                return;
            }
            await _repositoryManager.DictionaryRepository.DeleteByIdAsync(_dictionary.Id);
            await _botClient.SendTextMessageAsync(_user.ChatId, $"Словарь '{_dictionary.Name}' удален");
        }
    }
}
