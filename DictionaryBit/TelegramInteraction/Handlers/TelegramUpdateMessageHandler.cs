using DictionaryBit.TelegramInteraction.Operations;
using DictionaryBit.Data.Entities.Factories;
using DictionaryBit.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Handlers
{
    public sealed class TelegramUpdateMessageHandler : TelegramUpdateHandler
    {
        public TelegramUpdateMessageHandler(RepositoryManager repositoryManager, IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
            : base(repositoryManager, serviceProvider, httpContext)
        {}

        public override async Task HandleAsync(Update update)
        {
            var tgUser = update.Message.From;
            var user = _repositoryManager.UserRepository.GetByChatId(tgUser.Id);
            if (user == null)
            {
                user = UserFactory.Create(tgUser);
                await _repositoryManager.UserRepository.SaveAsync(user);
                return;//todo повторите запрос
            }
            var text = update.Message.Text;
            if (string.IsNullOrEmpty(text))
                return;
            var session = _httpContext.HttpContext?.Session;
            if (session == null)
                return;
            var command = CommandFactory.Create(_serviceProvider, session, text);
            await command?.ExecuteAsync(user, text);
        }
    }
}
