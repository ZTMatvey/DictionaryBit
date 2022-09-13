using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations
{
    public static class CommandFactory
    {
        public static CommandBase Create(IServiceProvider serviceProvider, ISession session, string command)
        {
            var commands = serviceProvider.GetServices<CommandBase>().ToList();
            var result = commands.FirstOrDefault(x => command.StartsWith(x.CommandName));
            if (result == null)
            {
                result = CheckOperaions(commands, session, command);
                if (result == null)
                    result = commands.First(x => x.CommandName == CommandNames.Default);
            }
            return result;
        }
        private static CommandBase CheckOperaions(IEnumerable<CommandBase> commands, ISession session, string command)
        {
            var currentOperation = session.Get<string>(SessionKeyNames.CurrentOperation);
            
            var result = currentOperation switch
            {
                CommandNames.SetActiveDictionary => commands.First(x => x.CommandName == CommandNames.SetActiveDictionary),
                //CommandNames.AddDictionaryName => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddDictionaryName),
                _ => null
            };
            return result;
            CommandBase slashCheck()
            {
                if (command.Contains("/"))
                {
                    var slashWarning = commands.First(x => x.CommandName == CommandNames.SymbolWarning);
                    return slashWarning;
                }
                return null;
            }
        }
    }
}
