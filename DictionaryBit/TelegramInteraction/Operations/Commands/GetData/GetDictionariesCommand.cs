using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DictionaryBit.TelegramInteraction.Operations.Commands
{
    public sealed class GetDictionariesCommand : CommandBase
    {
        public override string CommandName => CommandNames.GetDictionaries;
        public GetDictionariesCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }

        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь", replyMarkup: keyboard);
            _session.Set(SessionKeyNames.CurrentOperation, CommandNames.GetWords);
        }
    }
}
