using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary
{
    public class UseDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.UseDictionary;
        private readonly Operations.ActiveDictionary _activeDictionary;
        public UseDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, Operations.ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var withNamePattern = $@"^\{CommandName} ([^\/]+)$";
            var withNameRegex = new Regex(withNamePattern);
            var withNameMatch = withNameRegex.Match(content);
            IActiveDictionaryState state;
            string result;
            var commandState = session.Get<ActiveDictionaryState>(SessionKeyNames.UseDictionaryState);
            if (withNameMatch.Success)
            {
                var name = withNameMatch.Groups[1].Value;
                var dictionary = repositoryManager.DictionaryRepository.GetByName(name, user.Id);
                state = new SetActiveDictionaryState(botClient, session, user, dictionary, _activeDictionary, content);
                closeCommand();
            }
            else switch (commandState)
                {
                    case ActiveDictionaryState.None:
                        state = new SelectDictionaryState(repositoryManager, user, botClient);
                        session.Set(SessionKeyNames.UseDictionaryState, ActiveDictionaryState.SelectDictionary);
                        result = CommandNames.UseDictionary;
                        break;
                    case ActiveDictionaryState.SelectDictionary:
                        var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
                        var dictionary = repositoryManager.DictionaryRepository.GetById(dictionaryId);
                        state = new SetActiveDictionaryState(botClient, session, user, dictionary, _activeDictionary, content);
                        closeCommand();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            await state.ExecuteAsync();
            return result;
            void closeCommand()
            {
                session.Remove(SessionKeyNames.UseDictionaryState);
                result = string.Empty;
            }
        }
    }
}
