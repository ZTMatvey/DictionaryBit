using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public abstract class SaveWordBase : CommandBase
    {
        protected readonly WordInteraction _wordInteraction;
        public SaveWordBase(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext)
        {
            _wordInteraction = wordInteraction;
        }
        protected async Task TrySaveAsync(User user, Guid dictionaryId)
        {
            var result = await SaveWordAsync(_session, user, dictionaryId);
            switch (result)
            {
                case SaveWordResult.Ok:
                    RemoveWordDataFromSession();
                    await _botClient.SendTextMessageAsync(user.ChatId, "Слово сохранено");
                    break;
                case SaveWordResult.UserHasNoDictionary:
                    await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет такого словаря");
                    break;
                case SaveWordResult.DuplicateWord:
                    await _botClient.SendTextMessageAsync(user.ChatId, "Ошибка! Данное слово уже существует");
                    break;
            }
        }
        protected async Task<SaveWordResult> SaveWordAsync(ISession session, User user, Guid dictionaryId)//todo попробовать base session
        {
            var foreign = session.Get<string>("addWordForeign");
            var native = session.Get<string>("addWordNative");
            var description = session.Get<string>("addWordDescription");
            var isExist = _wordInteraction.CheckWordForExist(dictionaryId, foreign);
            if (isExist)
                return SaveWordResult.DuplicateWord;
            var word = new Word() { Foreign = foreign, Native = native, Description = description, DictionaryId = dictionaryId };
            var result = await _wordInteraction.SaveAsync(word, user);
            return result;
        }
        protected void RemoveWordDataFromSession()
        {
            _session.Remove(CommandNames.CurrentOperation);
            _session.Remove("addWordForeign");
            _session.Remove("addWordNative");
            _session.Remove("addWordDescription");
        }
    }
}
