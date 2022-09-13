using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.ActiveDictionary
{
    public class UseDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.UseDictionary;
        private readonly Operations.ActiveDictionary _activeDictionary;
        public UseDictionaryCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot, Operations.ActiveDictionary activeDictionary) : base(repositoryManager, httpContextAccessor, telegramBot)
        {
            _activeDictionary = activeDictionary;
        }
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var withNamePattern = $@"^\{CommandName} ([^\/]+)$";
            var withNameRegex = new Regex(withNamePattern);
            var withNameMatch = withNameRegex.Match(content);
            if (withNameMatch.Success)
            {
                var name = withNameMatch.Groups[1].Value;
                var dictionary = repositoryManager.DictionaryRepository.GetByName(name, user.Id);
                if (dictionary == default)
                    await botClient.SendTextMessageAsync(user.ChatId, $"У вас нет словаря '{name}'");
                else
                {
                    _activeDictionary.Set(session, dictionary);
                    await botClient.SendTextMessageAsync(user.ChatId, $"Словарь '{dictionary.Name}' теперь активен");
                }
                return string.Empty;
            }
            else
            {
                var dictionaryKeyboard = CommandHelper.GetDictionariesKeyboard(repositoryManager, user);
                await botClient.SendTextMessageAsync(user.ChatId, "Выберите словарь для активации", replyMarkup: dictionaryKeyboard);
                return CommandNames.SetActiveDictionary;
            }
        }
    }
}
