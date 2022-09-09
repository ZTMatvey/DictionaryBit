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

namespace DictionaryBit.BL.Operations.Commands
{
    public sealed class AddWordDescriptionCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddWordDescription;
        public AddWordDescriptionCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            var session = _httpContext.HttpContext?.Session;
            session.Set("addWordDescription", text);
            var isUsingDictionary = session.Keys.Contains("usedDictionaryId");
            var result = SaveWordResult.NotOk;
            if (isUsingDictionary)
                result = await SaveWord(session, user);
            if (result != SaveWordResult.Ok)
                ChooseDictionary(user);
        }
        private async void ChooseDictionary(Data.Entities.User user)
        {
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для сохранения", replyMarkup: keyboard);
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
