using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace DictionaryBit.TelegramInteraction.Operations.Command.GetEntities.GetWords.States
{
    public class PrintWordsState : IState
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly User _user;
        private readonly TelegramBotClient _botClient;
        private readonly Dictionary _dictionary;
        public PrintWordsState(RepositoryManager repositoryManager, User user, TelegramBotClient botClient, Dictionary dictionary)
        {
            _repositoryManager = repositoryManager;
            _user = user;
            _botClient = botClient;
            _dictionary = dictionary;
        }

        public async Task ExecuteAsync()
        {
            if (_dictionary == default)
            {
                await _botClient.SendTextMessageAsync(_user.ChatId, "У вас нет данного словаря");
                return;
            }
            var words = _repositoryManager.WordRepository.GetAllWordsByDictionaryId(_dictionary.Id).ToArray();
            if (words.Length == 0)
            {
                await _botClient.SendTextMessageAsync(_user.ChatId, $"Словарь '{_dictionary.Name}' пуст");
                return;
            }
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"{_dictionary.Name}:");
            for (int i = 0; i < words.Length; i++)
                messageBuilder.AppendLine($"{i + 1}. {words[i]}");
            var buttons = new InlineKeyboardButton[] {
                    new InlineKeyboardButton("Редактировать"){CallbackData="edit"},
                    new InlineKeyboardButton("Удалить"){CallbackData="delete"}};
            var keyboard = new InlineKeyboardMarkup(buttons);
            await _botClient.SendTextMessageAsync(_user.ChatId, messageBuilder.ToString(), replyMarkup: keyboard);
        }
    }
}
