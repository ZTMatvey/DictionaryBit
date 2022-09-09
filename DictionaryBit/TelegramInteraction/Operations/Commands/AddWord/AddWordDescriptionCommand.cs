using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
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
    public sealed class AddWordDescriptionCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddWordDescription;
        public AddWordDescriptionCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            _session.Set("addWordDescription", text);
            var isUsingDictionary = _session?.Keys.Contains("usedDictionaryId") ?? false;
            var result = SaveWordResult.NotOk;
            if (isUsingDictionary)
                result = await SaveWord(_session, user);
            if (result == SaveWordResult.Ok)
            {
                _session.Remove(CommandNames.CurrentOperation);
                _session.Remove("addWordForeign");
                _session.Remove("addWordNative");
                _session.Remove("addWordDescription");
            }
            else
            {
                var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
                await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для сохранения", replyMarkup: keyboard);
                _session.Set(CommandNames.CurrentOperation, CommandNames.SaveWord);
            }
        }
        private async void ChooseDictionary(Data.Entities.User user)
        {
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для сохранения", replyMarkup: keyboard);
            _session.Set(CommandNames.CurrentOperation, CommandNames.SaveWord);
        }
        private async Task<SaveWordResult> SaveWord(ISession session, Data.Entities.User user)
        {
            var foreign = session.Get<string>("addWordForeign");
            var native = session.Get<string>("addWordNative");
            var description = session.Get<string>("addWordDescription");
            var dictionaryId = session.Get<Guid>("usedDictionaryId");
            var userHasDictionary = _repositoryManager.UserRepository.HasDictionary(user.Id, dictionaryId);
            if (userHasDictionary)
            {
                var dictionary = _repositoryManager.DictionaryRepository.GetById(dictionaryId);
                var word = new Word() { Foreign = foreign, Native = native, Description = description, DictionaryId = dictionaryId };
                await _repositoryManager.WordRepository.SaveAsync(word);
                await _botClient.SendTextMessageAsync(user.ChatId, $"Слово сохранено в словарь \"{dictionary.Name}\"");
                return SaveWordResult.Ok;
            }
            return SaveWordResult.UserHasNotDictionary;
        }
    }
}
