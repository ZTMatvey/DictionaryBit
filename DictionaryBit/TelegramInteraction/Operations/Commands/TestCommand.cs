using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Session;

namespace DictionaryBit.TelegramInteraction.Operations.Commands
{
    public sealed class TestCommand : CommandBase
    {
        public override string CommandName => CommandNames.Test;
        public TestCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) 
            : base(telegramBot, repositoryManager, httpContext)
        {}
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var httpContext = _httpContext.HttpContext;
            var response = httpContext.Response;
            var request = httpContext.Request;
            var session = httpContext.Session;
            var rnd = new Random().Next(0, 10000);
            if (!session.Keys.Contains("LastString"))
            {
                await _botClient.SendTextMessageAsync(user.ChatId, $"Сессия установлена");
                session.SetString("LastString", rnd.ToString());
            }
            else
                await _botClient.SendTextMessageAsync(user.ChatId, $"Значения поля сессии: {session.Get("LastString")}");
        }
    }
}
