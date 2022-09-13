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
        protected async Task TrySaveAsync(User user)
        {
            var result = await SaveWordAsync(_session, user);
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
                case SaveWordResult.NotOk:
                    await _botClient.SendTextMessageAsync(user.ChatId, "Что-то пошло не так");
                    break;
            }
        }
        protected async Task<SaveWordResult> SaveWordAsync(ISession session, User user)//todo попробовать base session
        {
            string GetStringFromSessionOrEmpty(string name)
                => session.Get<string>(name)?.Trim() ?? string.Empty;
            var foreign = GetStringFromSessionOrEmpty(SessionKeyNames.AddEditWordForeign);
            var native = GetStringFromSessionOrEmpty(SessionKeyNames.AddEditWordNative);
            var description = GetStringFromSessionOrEmpty(SessionKeyNames.AddEditWordDescription);
            var id = _session.Get<Guid>(SessionKeyNames.AddEditWordId);
            var dictionaryId = _session.Get<Guid>(SessionKeyNames.AddEditDictionaryId);
            var isEditing = id != default;
            Word word;
            if (isEditing)
            {
                word = _repositoryManager.WordRepository.GetById(id);
                if (word == null)
                    return SaveWordResult.NotOk;
                word.Foreign = foreign;
                word.Native = native;
                word.Description = description;
            }
            else
            {
                var isExist = _wordInteraction.CheckWordForExist(dictionaryId, foreign);
                if (isExist)
                    return SaveWordResult.DuplicateWord;
                word = new Word() { Foreign = foreign, Native = native, Description = description, DictionaryId = dictionaryId, UserId = user.Id, Id = id };
            }
            var result = await _wordInteraction.SaveAsync(word, user);
            return result;
        }
        protected void RemoveWordDataFromSession()
        {
            _session.Remove(SessionKeyNames.CurrentOperation);
            _session.Remove(SessionKeyNames.AddEditWordForeign);
            _session.Remove(SessionKeyNames.AddEditWordNative);
            _session.Remove(SessionKeyNames.AddEditWordDescription);
            _session.Remove(SessionKeyNames.AddEditWordId);
            _session.Remove(SessionKeyNames.AddEditDictionaryId);
        }
    }
}
