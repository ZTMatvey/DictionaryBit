using DictionaryBit.TelegramInteraction.Operations.Commands;
using DictionaryBit.Service;
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
        private static bool CommandHasIgnoreAttribute(CommandBase command)
        {
            var attribute = Attribute.GetCustomAttribute(command.GetType(), typeof(IgnoreCommandAttribute));
            return attribute != null;
        }
        private static CommandBase CheckOperaions(IEnumerable<CommandBase> commands, ISession session, string command)
        {
            var currentOperation = session.Get<string>(SessionKeyNames.CurrentOperation);
            
            var result = currentOperation switch
            {
                CommandNames.AddDictionaryName => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddDictionaryName),
                CommandNames.AddWordForeign => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordForeign),
                CommandNames.AddWordNative => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordNative),
                CommandNames.AddWordDescription => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordDescription),
                CommandNames.SaveWord => commands.First(x=>x.CommandName == CommandNames.SaveWord),
                CommandNames.SetUsedDictionary => commands.First(x=>x.CommandName == CommandNames.SetUsedDictionary),
                _ => null
            };
            return result;
            CommandBase slashCheck()
            {
                if (command.Contains("/"))
                {
                    var slashWarning = commands.First(x => x.CommandName == CommandNames.SlashWarning);
                    return slashWarning;
                }
                return null;
            }
        }
    }
}
