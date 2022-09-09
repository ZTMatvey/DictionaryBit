using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands
{
    public sealed class SaveWordCommand : CommandBase
    {
        public override string CommandName => CommandNames.SaveWord;
        public SaveWordCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var pattern = @"^\/dictionaryName (.*)$";
            var regex = new Regex(pattern);
            var isMatch = regex.IsMatch(content);
            if (!isMatch)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var match = regex.Match(content);
            var name = match.Groups[1].Value;

            var session = _httpContext.HttpContext?.Session;
            var foreign = session.Get<string>("addWordForeign");
            var native = session.Get<string>("addWordNative");
            var description = session.Get<string>("addWordDescription");

            var allDictionaries = _repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(user.Id);
            var dictionary = allDictionaries.FirstOrDefault(x => x.Name == name);
            if (dictionary == null)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var userHasDictionary = _repositoryManager.UserRepository.HasDictionary(user.Id, dictionary.Id);
            if (userHasDictionary)
            {
                var word = new Word() { Foreign = foreign, Native = native, Description = description, DictionaryId = dictionary.Id };
                await _repositoryManager.WordRepository.SaveAsync(word);
                await _botClient.SendTextMessageAsync(user.ChatId, $"Слово сохранено в словарь \"{dictionary.Name}\"");
            }
            else
                await _botClient.SendTextMessageAsync(user.ChatId, $"У вас нет данного словаря");
        }
    }
}
