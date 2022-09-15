using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command
{
    public class RandomWordCommand : CommandBase
    {
        public override string CommandName => CommandNames.RandomWord;
        private readonly static Random _random = new();
        public RandomWordCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        {}
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var dictionaryName = CommandHelper.GetContentCommand(CommandName, content, "from ");
            IEnumerable<Word> words;
            if (dictionaryName == default)
                words = repositoryManager.WordRepository.GetAllWordsByUserId(user.Id);
            else
            {
                var dictionary = repositoryManager.DictionaryRepository.GetByName(dictionaryName, user.Id);
                if (dictionary == default)
                {
                    await botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                    return string.Empty;
                }
                words = repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionary.Id);
            }
            var count = words.Count();
            if(count == 0)
                return string.Empty;
            var skip = _random.Next(0, count);
            var wordToNoti = words.Skip(skip).First();
            await botClient.SendTextMessageAsync(user.ChatId, wordToNoti.ToString());
            return string.Empty;
        }
    }
}
