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
    public sealed class AddWordForeignCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddWordForeign;
        public AddWordForeignCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            _session?.Set("addWordForeign", text);
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на родном языке");
            _session.Set(CommandNames.CurrentOperation, CommandNames.AddWordNative);
        }
    }
}
