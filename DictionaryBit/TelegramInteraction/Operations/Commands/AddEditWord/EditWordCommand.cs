using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Commands.AddWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddEditWord
{
    public sealed class EditWordCommand : SaveWordBase
    {
        public override string CommandName => CommandNames.EditWord;
        public EditWordCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext, wordInteraction) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var onlyIdPattern = $"^\\{CommandNames.EditWord} in \"(.+)\" where Id=(\\d+)$";
            var onlyIdRegex = new Regex(onlyIdPattern);
            var onlyIdMatch = onlyIdRegex.Match(content);
            if (onlyIdMatch.Success)
            {
                await SetWordId(onlyIdMatch, user);
                await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на иностранном языке");
                _session.Set(SessionKeyNames.CurrentOperation, CommandNames.AddEditWordForeign);
            }
            else
            {
                var fullWordPattern = $"^\\{CommandNames.EditWord} in \"(.+)\" where Id=(\\d+) ([^\\/]+)\\/([^\\/]+)\\/([^\\/]+)$";
                var fullWordRegex = new Regex(fullWordPattern);
                var fullWordMatch = fullWordRegex.Match(content);
                if (fullWordMatch.Success)
                {
                    await SetWordId(fullWordMatch, user);
                    SetDataToSession(fullWordMatch.Groups);
                    await TrySaveAsync(user);
                }
                else
                    await ChooseDictionary(user);
            }
        }
        private async Task SetWordId(Match match, Data.Entities.User user)
        {
            var name = match.Groups[1].Value;
            var isParsed = int.TryParse(match.Groups[2].Value, out var wordLocalId);
            if (!isParsed)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "Ошибка при парсинге Id");
                return;
            }
            var dictionary = _repositoryManager.DictionaryRepository.GetByName(name, user.Id);
            if (dictionary == null)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var words = _repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionary.Id);
            var word = words.Skip(wordLocalId - 1).FirstOrDefault();
            _session.Set(SessionKeyNames.AddEditDictionaryId, dictionary.Id);
            _session.Set(SessionKeyNames.AddEditWordId, word?.Id ?? default);
        }
        private void SetDataToSession(GroupCollection group)
        {
            _session.Set(SessionKeyNames.AddEditWordForeign, group[3].Value);
            _session.Set(SessionKeyNames.AddEditWordNative, group[4].Value);
            _session.Set(SessionKeyNames.AddEditWordDescription, group[5].Value);
        }
        private async Task ChooseDictionary(Data.Entities.User user)
        {
            throw new NotImplementedException();
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь", replyMarkup: keyboard);
            _session.Set(SessionKeyNames.CurrentOperation, CommandNames.SaveWord);
        }
    }
}
