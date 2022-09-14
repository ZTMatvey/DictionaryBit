using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.AddDictionary.States
{
    public class SaveDictionaryState : IState
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly TelegramBotClient _botClient;
        private string _name;
        private readonly User _user;

        public SaveDictionaryState(RepositoryManager repositoryManager, TelegramBotClient botClient, User user, string name)
        {
            _repositoryManager = repositoryManager;
            _botClient = botClient;
            _user = user;
            _name = name;
        }
        public async Task ExecuteAsync()
        {
            _name = _name.Trim();
            var lowerName = _name.ToLower();
            var usersRepositories = _repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(_user.Id);
            var sameNameDictionary = usersRepositories.FirstOrDefault(x => x.Name.ToLower() == lowerName);
            if (sameNameDictionary != null)
            {
                await _botClient.SendTextMessageAsync(_user.ChatId, $"Словарь с таким именем уже существует");
                return;
            }
            var dictionary = new Dictionary()
            {
                UserId = _user.Id,
                Name = _name
            };
            await _repositoryManager.DictionaryRepository.SaveAsync(dictionary);
            await _botClient.SendTextMessageAsync(_user.ChatId, $"Создан словарь '{_name}'");
        }
    }
}
