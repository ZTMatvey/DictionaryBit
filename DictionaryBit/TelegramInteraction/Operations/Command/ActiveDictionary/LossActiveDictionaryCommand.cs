using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary
{
    public class LossActiveDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.LossDictionary;
        private readonly ActiveDictionary _activeDictionary;
        public LossActiveDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var activeDictionary = _activeDictionary.Get(session);
            if (activeDictionary == null)
                await botClient.SendTextMessageAsync(user.ChatId, "У вас нет активного словаря");
            else
            {
                _activeDictionary.Loss(session);
                await botClient.SendTextMessageAsync(user.ChatId, $"Словарь '{activeDictionary.Name}' больше не активен");
            }
            return string.Empty;
        }
    }
}
