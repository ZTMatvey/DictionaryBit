using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction
{
    public interface ITelegramBot: IDisposable
    {
        Task<TelegramBotClient> GetBot();
    }
}
