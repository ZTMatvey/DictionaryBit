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
    public sealed class AddEditWordForeignCommand : CommandBase
    {
        private readonly WordInteraction _wordInteraction;
        public override string CommandName => CommandNames.AddEditWordForeign;
        public AddEditWordForeignCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext)
        {
            _wordInteraction = wordInteraction;
        }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            var isUsingDictionary = _session?.Keys.Contains(SessionKeyNames.UsedDictionaryId) ?? false;
            if (isUsingDictionary)
            {
                var dictionaryId = _session.Get<Guid>(SessionKeyNames.UsedDictionaryId);
                var isExist = _wordInteraction.CheckWordForExist(dictionaryId, text);
                if (isExist)
                {
                    var isEditing = _session.Get<Guid>(SessionKeyNames.AddEditWordId) != default;
                    if (!isEditing)
                    {
                        await _botClient.SendTextMessageAsync(user.ChatId, "Ошибка! Данное слово уже существует");
                        _session.Remove(SessionKeyNames.CurrentOperation);
                        return;
                    }
                }
            }
            _session?.Set(SessionKeyNames.AddEditWordForeign, text);
            await _botClient.SendTextMessageAsync(user.ChatId, "Введите слово или фразу на родном языке");
            _session.Set(SessionKeyNames.CurrentOperation, CommandNames.AddEditWordNative);
        }
    }
}
