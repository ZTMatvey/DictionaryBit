﻿using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.TelegramInteraction.Operations.Commands.UseDictionary
{
    public sealed class SetUsedDictionaryCommand : CommandBase
    {
        public override string CommandName => CommandNames.SetUsedDictionary;
        public SetUsedDictionaryCommand(ITelegramBot telegramBot, RepositoryManager repositoryManager, IHttpContextAccessor httpContext) : base(telegramBot, repositoryManager, httpContext)
        { }
        public override async Task ExecuteAsync(Update update, Data.Entities.User user, string content)
        {
            var pattern = @$"^\{CommandNames.DictionaryNameData} (.*)$";
            var regex = new Regex(pattern);
            var isMatch = regex.IsMatch(content);
            if (!isMatch)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var match = regex.Match(content);
            Guid.TryParse(match.Groups[1].Value, out var dictionaryId);
            var dictionary = _repositoryManager.DictionaryRepository.GetById(dictionaryId);
            if (dictionary == null)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            _session.Set(SessionKeyNames.UsedDictionaryId, dictionary.Id);
            await _botClient.SendTextMessageAsync(user.ChatId, $"Словарь {dictionary.Name} теперь активен. Продолжайте комманду");
            _session.Remove(SessionKeyNames.CurrentOperation);
        }
    }
}
