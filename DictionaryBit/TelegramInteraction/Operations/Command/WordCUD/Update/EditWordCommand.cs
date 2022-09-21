using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Update.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Update
{
    public class EditWordCommand : CommandBase
    {
        public override string CommandName => CommandNames.EditWord;
        private readonly WordInteraction _wordInteraction;
        private readonly ActiveDictionary.ActiveDictionary _activeDictionary;
        public EditWordCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, WordInteraction wordInteraction, ActiveDictionary.ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _wordInteraction = wordInteraction;
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var pattern = @$"^{CommandName}(\s+in\s+(?<dictionary>[^/]+?))*(\s+where\s+id=(?<id>\d+))*(?<word>\s+(?<foreign>[^/]+)/(?<native>[^/]+)/(?<description>[^/]+))*$";
            var regex = new Regex(pattern);
            var match = regex.Match(content);
            if (!match.Success)
            {
                await botClient.SendTextMessageAsync(user.ChatId, "Ошибка! Чаще всего это сообщение возникает из-за символа '/' в строке");
                return string.Empty;
            }
            var wordConstraintsResult = await CheckForWordIdConstaintsAndAccept(user, match.Groups);
            if (wordConstraintsResult == WordIdConstaints.Error)
                return string.Empty;
            return string.Empty;
        }
        private async Task<WordIdConstaints> CheckForWordIdConstaintsAndAccept(User user, GroupCollection group)
        {
            var id = group["id"].Value;
            var dictionaryName = group["dictionary"].Value;
            var idWithoutDictionary = !string.IsNullOrEmpty(id) && string.IsNullOrEmpty(dictionaryName);
            if (string.IsNullOrEmpty(id))
            {
                if (string.IsNullOrEmpty(dictionaryName))
                    return WordIdConstaints.NotSetted;
                else
                {
                    var dictionary = getDictionary();
                    if (dictionary == default)
                    {
                        await botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                        return WordIdConstaints.Error;
                    }
                    session.Set(SessionKeyNames.EditingWordDictionaryId, dictionary.Id);
                    return WordIdConstaints.SettedOnlyDictionaryId;
                }
            }
            if (idWithoutDictionary)
            {
                var activeDictionary = _activeDictionary.Get(session);
                if (activeDictionary == null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId, "Ошибка! Id слова может использоваться только одновременно со словарем");
                    return WordIdConstaints.Error;
                }
            }
            {
                var dictionary = getDictionary();
                if (dictionary == default)
                    dictionary = _activeDictionary.Get(session);
                var words = repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionary.Id);
                var isParsed = int.TryParse(group["id"].Value, out var wordLocalId);
                if (!isParsed)
                {
                    await botClient.SendTextMessageAsync(user.ChatId, "Ошибка при парсинге id");
                    return WordIdConstaints.Error;
                }
                var word = words.Skip(wordLocalId - 1).FirstOrDefault();
                if (word == null)
                {
                    await botClient.SendTextMessageAsync(user.ChatId, "Ошибка! У вас нет слова с таким Id");
                    return WordIdConstaints.Error;
                }
                session.Set(SessionKeyNames.EditingWordId, word.Id);
            }
            return WordIdConstaints.Setted;
            Dictionary? getDictionary()
            {
                return repositoryManager.DictionaryRepository.GetByName(dictionaryName, user.Id);
            }
        }
        private void SetInfoToSession(GroupCollection group)
        {
            session.Set(SessionKeyNames.WordForeign, group["foreign"].Value);
            session.Set(SessionKeyNames.WordNative, group["native"].Value);
            session.Set(SessionKeyNames.WordDescription, group["description"].Value);
        }
        private async Task<bool> AllInfoCommand(Match match, User user)
        {
            var dictionaryName = match.Groups["dictionaryName"].Value;
            var dictionary = repositoryManager.DictionaryRepository.GetByName(dictionaryName, user.Id);
            if (dictionary == default)
            {
                await botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return false;
            }
            return await BasicInfoCommand(match, user, dictionary);
        }
        private async Task<bool> BasicInfoCommand(Match match, User user, Dictionary dictionary)
        {
            var parsed = int.TryParse(match.Groups["wordLocalId"].Value, out var localId);
            if (!parsed)
            {
                await botClient.SendTextMessageAsync(user.ChatId, "Ошибка при парсинге id");
                return false;
            }
            var word = CommandHelper.GetWordFromLocalId(repositoryManager, dictionary.Id, localId);
            if (word == default)
            {
                await botClient.SendTextMessageAsync(user.ChatId, $"В словаре '{dictionary.Name}' нет данного слова");
                return false;
            }
            session.Set(SessionKeyNames.WordDictionaryId, dictionary.Id);
            session.Set(SessionKeyNames.EditingWordId, word.Id);
            SetInfoToSession(match.Groups);
            return true;
        }
        private Match GetRegexMatchFromContent(string pattern, string content)
        {
            return new Regex(pattern).Match(content);
        }
    }
}
