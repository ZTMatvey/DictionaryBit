using DictionaryBit.Data.Entities;
using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.WordCUD.Create.States
{
    public class EnterDescriptionState : IState
    {
        private readonly User _user;
        private readonly TelegramBotClient _botClient;
        public EnterDescriptionState(User user, TelegramBotClient botClient)
        {
            _user = user;
            _botClient = botClient;
        }
        public async Task ExecuteAsync()
        {
            await _botClient.SendTextMessageAsync(_user.ChatId, "Введите описание слова");
        }
    }
}
