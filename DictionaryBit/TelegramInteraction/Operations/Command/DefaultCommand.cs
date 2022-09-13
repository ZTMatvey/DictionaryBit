using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command
{
    public class DefaultCommand : CommandBase
    {
        public override string CommandName => CommandNames.Default;
        public DefaultCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        {}
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            await botClient.SendTextMessageAsync(user.ChatId, "Я не понял комманду. Возможно, ваша сессия истекла, если так, то начните операцию сначала");
            return string.Empty;
        }
    }
}
