using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Create.States;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Create
{
    public class AddWordCommand : CommandBase
    {
        private readonly WordInteraction _wordInteraction;
        public override string CommandName => CommandNames.AddWord;
        private readonly ActiveDictionary.ActiveDictionary _activeDictionary;
        public AddWordCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, WordInteraction wordInteraction, ActiveDictionary.ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _wordInteraction = wordInteraction;
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var wordWithDictionaryPattern = $@"^\{CommandName} ([^\/]+)\/([^\/]+)\/([^\/]+) to ([^\/]+)$";
            var onlyWordPattern = $@"^\{CommandName} ([^\/]+)\/([^\/]+)\/([^\/]+)$";
            var regex = new Regex(wordWithDictionaryPattern);
            var withDictionary = regex.Match(content);
            IState state;
            string result;
            var commandState = session.Get<AddWord>(CommandName);
            if (withDictionary.Success)
            {
                SetWordDataToSession(withDictionary);
                var dictionary = repositoryManager.DictionaryRepository.GetByName(withDictionary.Groups[4].Value, user.Id);
                if (dictionary == default)
                {
                    await botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                    return string.Empty;
                }
                closeCommand(dictionary);
            }
            else
            {
                regex = new Regex(onlyWordPattern);
                var onlyWord = regex.Match(content);
                result = CommandName;
                if (onlyWord.Success)
                {
                    SetWordDataToSession(onlyWord);
                    selectDictionaryState();
                }
                else switch (commandState)
                    {
                        case AddWord.None:
                            state = new EnterForeignState(user, botClient);
                            session.Set(CommandName, AddWord.Foreign);
                            break;
                        case AddWord.Foreign:
                            state = new EnterNativeState(user, botClient);
                            session.Set(CommandName, AddWord.Native);
                            session.Set(SessionKeyNames.WordForeign, content);
                            break;
                        case AddWord.Native:
                            state = new EnterDescriptionState(user, botClient);
                            session.Set(CommandName, AddWord.Description);
                            session.Set(SessionKeyNames.WordNative, content);
                            break;
                        case AddWord.Description:
                            selectDictionaryState();
                            session.Set(SessionKeyNames.WordDescription, content);
                            break;
                        case AddWord.SelectDictionary:
                            var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
                            var dictionary = repositoryManager.DictionaryRepository.GetById(dictionaryId);
                            closeCommand(dictionary);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
            }
            await state.ExecuteAsync();
            return result;
            void selectDictionaryState()
            {
                var activeDictionary = _activeDictionary.Get(session);
                if (activeDictionary == default)
                {
                    state = new SelectDictionaryState(repositoryManager, user, botClient, ", куда хотите добавить слово");
                    session.Set(CommandName, AddWord.SelectDictionary);
                    return;
                }
                closeCommand(activeDictionary);
            }
            void closeCommand(Dictionary dictionary)
            {
                session.Set(SessionKeyNames.WordDictionaryId, dictionary.Id);
                state = new AddWordState(repositoryManager, _wordInteraction, botClient, session, user);
                result = string.Empty;
                session.Remove(CommandName);
            }
        }
        private void SetWordDataToSession(Match match)
        {
            session.Set(SessionKeyNames.WordForeign, match.Groups[1].Value);
            session.Set(SessionKeyNames.WordNative, match.Groups[2].Value);
            session.Set(SessionKeyNames.WordDescription, match.Groups[3].Value);
        }
    }
}
