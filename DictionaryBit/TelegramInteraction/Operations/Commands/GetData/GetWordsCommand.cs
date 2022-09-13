using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.GetData
{
    public sealed class GetWordsCommand : CommandBase
    {
        public override string CommandName => CommandNames.GetWords;
        public GetWordsCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        {}
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var dictionaryId = CommandHelper.GetDictionaryIdOrDefault(content);
            var dictionary = _repositoryManager.DictionaryRepository.CheckAndGetById(dictionaryId, user.Id);
            if (dictionary == default)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var words = _repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionaryId).ToArray();
            var messageBuilder = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
                messageBuilder.AppendLine($"{i + 1}. {words[i]}");
            var buttons = new InlineKeyboardButton[] {
                    new InlineKeyboardButton("Редактировать"){SwitchInlineQueryCurrentChat=$"{CommandNames.EditWord} in \"{dictionary.Name}\" where Id="},
                    new InlineKeyboardButton("Удалить"){SwitchInlineQueryCurrentChat=$"{CommandNames.DeleteWord} in \"{dictionary.Name}\" where Id="}};
            var keyboard = new InlineKeyboardMarkup(buttons);
            await _botClient.SendTextMessageAsync(user.ChatId, messageBuilder.ToString(), replyMarkup: keyboard);
        }
    }
}
