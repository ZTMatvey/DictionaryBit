using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.AddDictionary.States;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.AddDictionary
{
    public class AddDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddDictionary;
        public AddDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        { }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var name = CommandHelper.GetContentCommand(CommandName, content);
            IState state;
            string result;
            var commandState = session.Get<Enums.AddDictionary>(CommandName);
            if (name != null)
                closeCommand(name);
            else switch (commandState)
                {
                    case Enums.AddDictionary.None:
                        state = new EnterDictionaryNameState(botClient, user);
                        session.Set(CommandName, Enums.AddDictionary.EnterName);
                        result = CommandName;
                        break;
                    case Enums.AddDictionary.EnterName:
                        closeCommand(content);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            await state.ExecuteAsync();
            return result;
            void closeCommand(string name)
            {
                state = new SaveDictionaryState(repositoryManager, botClient, user, name);
                session.Remove(CommandName);
                result = string.Empty;
            }
        }
    }
}
