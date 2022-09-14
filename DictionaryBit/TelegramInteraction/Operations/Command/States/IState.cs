using DictionaryBit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command.States
{
    public interface IState
    {
        Task ExecuteAsync();
    }
}
