using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace DictionaryBit.Service
{
    public static class CommandHelper
    {
        public static InlineKeyboardMarkup GetDictionariesKeyboard(RepositoryManager repositoryManager, User user)
        {
            var dictionaries = repositoryManager.DictionaryRepository.GetAllDictionariesByUserId(user.Id);
            var buttons = new List<InlineKeyboardButton[]>();
            foreach (var dictionary in dictionaries)
            {
                var button = new[] { new InlineKeyboardButton(dictionary.Name) { CallbackData = $"/dictionaryName {dictionary.Id}" } };
                buttons.Add(button);
            }
            var keyboard = new InlineKeyboardMarkup(buttons);
            return keyboard;
        }
    }
}
