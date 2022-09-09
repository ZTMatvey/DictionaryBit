using DictionaryBit.BL.Operations.Commands;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.BL.Operations
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
            if(!CommandHasIgnoreAttribute(result))
                session.Set("LastCommand", result.CommandName);
            return result;
        }
        private static bool CommandHasIgnoreAttribute(CommandBase command)
        {
            var attribute = Attribute.GetCustomAttribute(command.GetType(), typeof(IgnoreCommandAttribute));
            return attribute != null;
        }
        private static CommandBase CheckOperaions(IEnumerable<CommandBase> commands, ISession session, string command)
        {
            var lastCommand = session.Get<string>("LastCommand");
            CommandBase slashCheck(){
                if (command.Contains("/"))
                {
                    var slashWarning = commands.First(x => x.CommandName == CommandNames.SlashWarning);
                    return slashWarning;
                }
                return null;
            }
            
            var result = lastCommand switch
            {
                CommandNames.AddDictionary => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddDictionaryName),
                CommandNames.AddWord => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordForeign),
                CommandNames.AddWordForeign => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordNative),
                CommandNames.AddWordNative => slashCheck() ?? commands.First(x=>x.CommandName == CommandNames.AddWordDescription),
                CommandNames.AddWordDescription => commands.First(x=>x.CommandName == CommandNames.SaveWord),
                CommandNames.UseDicrionary => commands.First(x=>x.CommandName == CommandNames.SetUsedDictionary),
                _ => null
            };
            return result;
        }
    }
}
