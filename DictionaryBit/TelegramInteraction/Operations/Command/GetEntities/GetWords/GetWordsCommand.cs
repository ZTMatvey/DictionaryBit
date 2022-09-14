using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.GetEntities.GetWords.States;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.TelegramInteraction.Operations.Command.GetEntities.GetWords
{
    public class GetWordsCommand : CommandBase
    {
        public override string CommandName => CommandNames.GetWords;
        public GetWordsCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        { }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var dictionaryName = CommandHelper.GetNameFromCommand(CommandName, content);
            IState state;
            string result;
            var commandState = session.Get<DictionarySelect>(CommandName);
            if (dictionaryName != null)
            {
                var dictionary = repositoryManager.DictionaryRepository.GetByName(dictionaryName, user.Id);
                closeCommand(dictionary);
            }
            else switch (commandState)
                {
                    case DictionarySelect.None:
                        state = new SelectDictionaryState(repositoryManager, user, botClient, "для получения слов");
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
                state = new PrintWordsState(repositoryManager, user, botClient, dictionary);
                result = string.Empty;
                session.Remove(CommandName);
            }
        }
    }
}
