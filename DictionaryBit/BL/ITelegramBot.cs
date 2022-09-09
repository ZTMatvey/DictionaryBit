using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.BL
{
    public interface ITelegramBot: IDisposable
    {
        Task<TelegramBotClient> GetBot();
    }
}
