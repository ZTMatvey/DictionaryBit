using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public sealed class SaveWordCommand : SaveWordBase
    {
        public override string CommandName => CommandNames.SaveWord;
        public SaveWordCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext, wordInteraction)
        { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
            if (dictionaryId == default)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            _session.Set(SessionKeyNames.AddEditDictionaryId, dictionaryId);
            await TrySaveAsync(user);
        }
    }
}
