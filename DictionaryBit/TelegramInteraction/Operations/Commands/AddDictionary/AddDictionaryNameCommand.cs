using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddDictionary
{
    public sealed class AddDictionaryNameCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddDictionaryName;
        public AddDictionaryNameCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var name = content;
            var lowerName = name.ToLower();
            var usersRepositories = _repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(user.Id);
            var sameNameDictionary = usersRepositories.FirstOrDefault(x => x.Name.ToLower() == lowerName);
            if (sameNameDictionary != null)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, $"Словарь с таким именем уже существует");
                return;
            }
            var dictionary = new Data.Entities.Dictionary()
            {
                UserId = user.Id,
                Name = name
            };
            await _repositoryManager.DictionaryRepository.SaveAsync(dictionary);
            await _botClient.SendTextMessageAsync(user.ChatId, $"Создан словарь \"{name}\"");
            _session.Remove(SessionKeyNames.CurrentOperation);
        }
    }
}
