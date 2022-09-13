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
    public abstract class CommandBase
    {
        protected RepositoryManager repositoryManager;
        protected HttpContext httpContent;
        protected TelegramBotClient botClient;
        protected ISession session => httpContent.Session;
        public abstract string CommandName { get; }
        public CommandBase(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot)
        {
            this.repositoryManager = repositoryManager;
            httpContent = httpContextAccessor.HttpContext ?? throw new Exception();
            botClient = telegramBot.GetBot().Result;
        }
        public async Task ExecuteAsync(User user, string content)
        {
            var nextOperation = await ExecuteAndGetNextOperationAsync(user, content);
            if (string.IsNullOrEmpty(nextOperation))
            {
                var attribute = Attribute.GetCustomAttribute(GetType(), typeof(IgnoreCommandAttribute));
                if(attribute == null)
                    session.Remove(SessionKeyNames.CurrentOperation);
            }
            else
                session.Set(SessionKeyNames.CurrentOperation, nextOperation);
        }
        protected abstract Task<string> ExecuteAndGetNextOperationAsync(User user, string content);
    }
}
