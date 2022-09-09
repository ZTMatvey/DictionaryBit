using DictionaryBit.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.BL.Operations.Commands
{
    public abstract class CommandBase//todo подумать как можно избежать большого количества повторяющегося кода в классах наследниках
    {
        protected readonly TelegramBotClient _botClient;
        protected readonly RepositoryManager _repositoryManager;
        protected readonly IHttpContextAccessor _httpContext;
        protected ISession _session => _httpContext.HttpContext?.Session;
        public abstract string CommandName { get; }
        public CommandBase(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
        {
            _botClient = telegramBot.GetBot().Result;
            _repositoryManager = repositoryManager;
            _httpContext = httpContext;
        }
        public abstract Task ExecuteAsync(Update update, Data.Entities.User user, string content);
    }
}
