using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.GetEntities.GetDictionaries
{
    public class GetDictionariesCommand : CommandBase
    {
        public override string CommandName => CommandNames.GetDictionaries;
        public GetDictionariesCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        { }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var dictionaries = repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(user.Id);
            var response = string.Join('\n', dictionaries);
            await botClient.SendTextMessageAsync(user.ChatId, $"Словари:\n{response}");
            return string.Empty;
        }
    }
}
