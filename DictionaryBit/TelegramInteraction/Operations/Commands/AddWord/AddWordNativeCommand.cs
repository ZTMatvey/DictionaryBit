using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public sealed class AddWordNativeCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddWordNative;
        public AddWordNativeCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            _httpContext.HttpContext?.Session.Set("addWordNative", text);
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите описание слова");
        }
    }
}
