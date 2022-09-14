using DictionaryBit.Data.Entities;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.DictionaryManagment.AddDictionary.States
{
    public class EnterDictionaryNameState : IState
    {
        private readonly TelegramBotClient _botClient;
        private User _user;

        public EnterDictionaryNameState(TelegramBotClient botClient, User user)
        {
            _botClient = botClient;
            _user = user;
        }
        public async Task ExecuteAsync()
        {
            await _botClient.SendTextMessageAsync(_user.ChatId, "Введите название для словаря");
        }
    }
}
