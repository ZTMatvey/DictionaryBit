using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Interaction;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Create.States
{
    public class AddWordState : IState
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly ISession _session;
        private readonly User _user;
        private readonly WordInteraction _wordInteraction;
        private readonly TelegramBotClient _botClient;
        public AddWordState(RepositoryManager repositoryManager, WordInteraction wordInteraction, TelegramBotClient botClient, ISession session, User user)
        {
            _session = session;
            _user = user;
            _wordInteraction = wordInteraction;
            _repositoryManager = repositoryManager;
            _botClient = botClient;
        }

        public async Task ExecuteAsync()
        {
            var foreign = _session.Get<string>(SessionKeyNames.WordForeign) ?? throw new InvalidOperationException();
            var native = _session.Get<string>(SessionKeyNames.WordNative) ?? throw new InvalidOperationException();
            var description = _session.Get<string>(SessionKeyNames.WordDescription) ?? throw new InvalidOperationException();
            var dictionaryId = _session.Get<Guid>(SessionKeyNames.WordDictionaryId);
            if (dictionaryId == default)
                throw new InvalidOperationException();
            var word = new Word { Foreign = foreign, Native = native, Description = description, DictionaryId = dictionaryId, UserId = _user.Id};
            var result = await _wordInteraction.SaveAsync(word, _user);
            switch (result)
            {
                case Enums.SaveWordResult.Ok:
                    var dictionary = _repositoryManager.DictionaryRepository.GetById(dictionaryId);
                    await _botClient.SendTextMessageAsync(_user.ChatId, $"Слово сохранено в словарь '{dictionary.Name}'");
                    break;
                case Enums.SaveWordResult.NotOk:
                    await _botClient.SendTextMessageAsync(_user.ChatId, $"При сохранении что-то пошло не так");
                    break;
                case Enums.SaveWordResult.UserHasNoDictionary:
                    await _botClient.SendTextMessageAsync(_user.ChatId, $"У вас нет данного словаря");
                    break;
                case Enums.SaveWordResult.DuplicateWord:
                    await _botClient.SendTextMessageAsync(_user.ChatId, $"Данное слово уже существует");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
