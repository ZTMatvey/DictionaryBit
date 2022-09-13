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
    public class StartCommand : CommandBase
    {
        public override string CommandName => CommandNames.Start;
        public StartCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        {}
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            await botClient.SendTextMessageAsync(user.ChatId, $"Привет, ${user.UserName}. Я— бот, помогающий в изучении иностранных языков");
            return string.Empty;
        }
    }
}
