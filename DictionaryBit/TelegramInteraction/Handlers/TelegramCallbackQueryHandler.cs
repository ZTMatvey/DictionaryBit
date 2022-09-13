using DictionaryBit.TelegramInteraction.Operations;
using DictionaryBit.Data.Entities.Factories;
using DictionaryBit.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Handlers
{
    public sealed class TelegramCallbackQueryHandler : TelegramUpdateHandler
    {
        private readonly TelegramBotClient _botClient;
        public TelegramCallbackQueryHandler(RepositoryManager repositoryManager, TelegramBotClient botClient, IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
            : base(repositoryManager, serviceProvider, httpContext)
        {
            _botClient = botClient;
        }

        public override async Task HandleAsync(Update update)
        {
            var callback = update.CallbackQuery;
            var tgUser = callback.From;
            var user = _repositoryManager.UserRepository.GetByChatId(tgUser.Id);
            if (user == null)
            {
                user = UserFactory.Create(tgUser);
                await _repositoryManager.UserRepository.SaveAsync(user);
                return;//todo повторите запрос
            }
            var session = _httpContext.HttpContext?.Session;
            if (session == null)
                return;
            var command = CommandFactory.Create(_serviceProvider, session, callback.Data);
            await command?.ExecuteAsync(user, callback.Data);
            await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
        }
    }
}
