using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.RemoveDictionary.States;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.RemoveDictionary
{
    public class RemoveDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.RemoveDictionary;
        public RemoveDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        {}
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var name = CommandHelper.GetNameFromCommand(CommandName, content);
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
                        state = new SelectDictionaryState(repositoryManager, user, botClient, "для удаления");
                        result = CommandName;
                        session.Set(CommandName, DictionarySelect.SelectDictionary);
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
                state = new RemoveDictionaryState(repositoryManager, botClient, user, dictionary);
                result = string.Empty;
                session.Remove(CommandName);
            }
        }
    }
}
