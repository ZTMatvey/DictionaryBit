using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddDictionary
{
    public sealed class AddDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.AddDictionary;
        public AddDictionaryCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext)
            : base(telegramBot, repositoryManager, httpContext) { }

        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите название для словаря");
            _session.Set(SessionKeyNames.CurrentOperation, CommandNames.AddDictionaryName);
        }
    }
}
