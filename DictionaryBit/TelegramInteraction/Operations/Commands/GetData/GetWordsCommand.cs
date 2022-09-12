using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

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
            if (dictionaryId == default)
            {
                await _botClient.SendTextMessageAsync(user.ChatId, "У вас нет данного словаря");
                return;
            }
            var words = _repositoryManager.WordRepository.GetAllWordsByDictionaryId(dictionaryId);
            var message = string.Join('\n', words);
            await _botClient.SendTextMessageAsync(user.ChatId, message);
        }
    }
}
