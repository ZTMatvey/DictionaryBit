using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary.States;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
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
        private readonly ActiveDictionary _activeDictionary;
        public UseDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var name = CommandHelper.GetContentCommand(CommandName, content);
            IState state;
            string result;
            var commandState = session.Get<DictionarySelect>(CommandName);
            if (name != null)
            {
                var dictionary = repositoryManager.DictionaryRepository.GetByName(name, user.Id);
                closeCommand(dictionary);
            }
            else switch (commandState)
                {
                    case DictionarySelect.None:
                        state = new SelectDictionaryState(repositoryManager, user, botClient, "для активации");
                        session.Set(CommandName, DictionarySelect.SelectDictionary);
                        result = CommandName;
                        break;
                    case DictionarySelect.SelectDictionary:
                        var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
                        var dictionary = repositoryManager.DictionaryRepository.GetById(dictionaryId);
                        closeCommand(dictionary);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            await state.ExecuteAsync();
            return result;
            void closeCommand(Dictionary dictionary)
            {
                state = new SetActiveDictionaryState(botClient, session, user, dictionary, _activeDictionary);
                result = string.Empty;
                session.Remove(CommandName);
            }
        }
    }
}
