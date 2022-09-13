using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary
{
    public class SetActiveDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.SetActiveDictionary;
        private readonly Operations.ActiveDictionary _activeDictionary;
        public SetActiveDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, Operations.ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
            var dictionary = repositoryManager.DictionaryRepository.GetById(dictionaryId);
            if (dictionary == null || dictionary.UserId != user.Id)
                await botClient.SendTextMessageAsync(user.ChatId, $"У вас нет данного словаря");
            else
            {
                _activeDictionary.Set(session, dictionary);
                await botClient.SendTextMessageAsync(user.ChatId, $"Словарь '{dictionary.Name}' теперь активен");
            }
            return string.Empty;
        }
    }
}
