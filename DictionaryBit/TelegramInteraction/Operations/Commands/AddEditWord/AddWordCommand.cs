using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public class AddWordCommand : SaveOrChooseDictionaryBase
    {
        public override string CommandName => CommandNames.AddWord;
        public AddWordCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext, wordInteraction) { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            _session.Set(SessionKeyNames.AddEditWordId, default(Guid));
            var pattern = @$"\{CommandNames.AddWord} ([^\/]+)\/([^\/]+)\/([^\/]+)";
            var regex = new Regex(pattern);
            var match = regex.Match(content);
            if (match.Success)
            {
                SetDataToSession(match.Groups);
                await base.ExecuteAsync(update, user, content);
            }
            else
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на иностранном языке");
                _session.Set(SessionKeyNames.CurrentOperation, CommandNames.AddEditWordForeign);
            }
        }
        private void SetDataToSession(GroupCollection group)
        {
            _session.Set(SessionKeyNames.AddEditWordForeign, group[1].Value);
            _session.Set(SessionKeyNames.AddEditWordNative, group[2].Value);
            _session.Set(SessionKeyNames.AddEditWordDescription, group[3].Value);
        }
    }
}
