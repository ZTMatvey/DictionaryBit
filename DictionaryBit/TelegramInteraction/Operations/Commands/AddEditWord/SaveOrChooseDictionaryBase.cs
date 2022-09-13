using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public abstract class SaveOrChooseDictionaryBase : SaveWordBase
    {
        protected SaveOrChooseDictionaryBase(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction) : base(telegramBot, repositoryManager, httpContext, wordInteraction)
        {}
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var isUsingDictionary = _session?.Keys.Contains(SessionKeyNames.UsedDictionaryId) ?? false;
            if (isUsingDictionary)
            {
                var dictionaryId = _session.Get<Guid>(SessionKeyNames.UsedDictionaryId);
                _session.Set(SessionKeyNames.AddEditDictionaryId, dictionaryId);
                await TrySaveAsync(user);
            }
            else
                await ChooseDictionary(user);
        }
        private async Task ChooseDictionary(Data.Entities.User user)
        {
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для сохранения", replyMarkup: keyboard);
            _session.Set(SessionKeyNames.CurrentOperation, CommandNames.SaveWord);
        }
    }
}
