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
    public sealed class AddWordCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddWord;
        public AddWordCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на иностранном языке");
            _session.Set(CommandNames.CurrentOperation, CommandNames.AddWordForeign);
        }
    }
}
