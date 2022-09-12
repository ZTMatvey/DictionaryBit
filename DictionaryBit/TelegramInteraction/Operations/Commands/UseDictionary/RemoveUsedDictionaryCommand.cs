using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.UseDictionary
{
    public sealed class RemoveUsedDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.RemoveUsedDictionary;
        public RemoveUsedDictionaryCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        {}

        public async override Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var dictionaryId = _session.Get<Guid>("usedDictionaryId");
            if (dictionaryId == default)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, $"У вас нет активного словаря");
                return;
            }
            var dictionary = _repositoryManager.DictionaryRepository.GetById(dictionaryId);
            _session.Remove("usedDictionaryId");
            await _botClient.SendTextMessageAsync(user.ChatId, $"Словарь {dictionary.Name} теперь неактивен. Продолжайте комманду");
        }
    }
}
