using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.AddWord
{
    public sealed class AddWordForeignCommand : CommandBase
    {
        private readonly WordInteraction _wordInteraction;
        public override string CommandName => CommandNames.AddWordForeign;
        public AddWordForeignCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext)
        {
            _wordInteraction = wordInteraction;
        }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            var isUsingDictionary = _session?.Keys.Contains("usedDictionaryId") ?? false;
            if (isUsingDictionary)
            {
                var dictionaryId = _session.Get<Guid>("usedDictionaryId");
                var isExist = _wordInteraction.CheckWordForExist(dictionaryId, text);
                if (isExist)
                {
                    await _botClient.SendTextMessageAsync(user.ChatId, "Ошибка! Данное слово уже существует");
                    _session.Remove(CommandNames.CurrentOperation);
                    return;
                }
            }
            _session?.Set("addWordForeign", text);
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на родном языке");
            _session.Set(CommandNames.CurrentOperation, CommandNames.AddWordNative);
        }
    }
}
