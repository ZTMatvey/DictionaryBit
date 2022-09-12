using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
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
    public sealed class AddWordDescriptionCommand : SaveWordBase
    {
        public override string CommandName => CommandNames.AddWordDescription;
        public AddWordDescriptionCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext, WordInteraction wordInteraction)
            : base(telegramBot, repositoryManager, httpContext, wordInteraction)
        { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var text = content;
            _session.Set("addWordDescription", text);
            var isUsingDictionary = _session?.Keys.Contains("usedDictionaryId") ?? false;
            if (isUsingDictionary)
            {
                var dictionaryId = _session.Get<Guid>("usedDictionaryId");
                await TrySaveAsync(user, dictionaryId);
            }
            else
                await ChooseDictionary(user);
        }
        private async Task ChooseDictionary(Data.Entities.User user)
        {
            var keyboard = CommandHelper.GetDictionariesKeyboard(_repositoryManager, user);
            await _botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для сохранения", replyMarkup: keyboard);
            _session.Set(CommandNames.CurrentOperation, CommandNames.SaveWord);
        }
    }
}