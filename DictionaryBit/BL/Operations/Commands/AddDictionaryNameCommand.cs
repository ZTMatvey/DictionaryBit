using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.BL.Operations.Commands
{
    public sealed class AddDictionaryNameCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddDictionaryName;
        public AddDictionaryNameCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        {}
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var name = content;
            var usersRepositories = _repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(user.Id);
            var sameNameRepository = usersRepositories.FirstOrDefault(x => x.Name == name);
            if (sameNameRepository != null)
                return;
            var dictionary = new Data.Entities.Dictionary()
            {
                UserId = user.Id,
                Name = name
            };
            await _repositoryManager.DictionaryRepository.SaveAsync(dictionary);
            await _botClient.SendTextMessageAsync(user.ChatId, $"Создан словарь \"{name}\"");
        }
    }
}
