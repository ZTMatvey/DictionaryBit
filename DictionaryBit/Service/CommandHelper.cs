using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
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
                var button = new[] { new InlineKeyboardButton(dictionary.Name) { CallbackData = $"{CommandNames.DictionaryIdData} {dictionary.Id}" } };
                buttons.Add(button);
            }
            var keyboard = new InlineKeyboardMarkup(buttons);
            return keyboard;
        }
        public static Guid GetDictionaryIdOrDefault(string input)
        {
            var pattern = @$"^\{CommandNames.DictionaryIdData} " + "([{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?)$";
            var regex = new Regex(pattern);
            var match = regex.Match(input);
            if (!match.Success)
                return default;
            var result = Guid.Parse(match.Groups[1].Value);
            return result;
        }
        public static string? GetContentCommand(string commandName, string input, string preposition = "")
        {
            var pattern = $@"^\{commandName} {preposition}([^\/]+)$";
            var regex = new Regex(pattern);
            var match = regex.Match(input);
            if(match.Success && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
                return match.Groups[1].Value;
            return null;
        }
        public static Word? GetWordFromLocalId(RepositoryManager repositoryManager, Guid dictionaryId, int localId)
        {
            var words = repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionaryId);
            return words.Skip(localId - 1).FirstOrDefault();
        }
    }
}
