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
    public sealed class DefaultCommand : CommandBase
    {
        public override string CommandName => CommandNames.Default;
        public DefaultCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        {}
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Я не понял комманду. Возможно, ваша сессия истекла, если так, то начните комманду сначала");
        }
    }
}
