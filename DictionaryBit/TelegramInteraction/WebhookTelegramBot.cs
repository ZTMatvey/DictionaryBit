using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction
{
    public sealed class WebhookTelegramBot: ITelegramBot
    {
        private readonly Config _config;
        private TelegramBotClient _botClient;
        public WebhookTelegramBot(Config config)
        {
            _config = config;
        }

        public async void Dispose()
        {
            await _botClient.CloseAsync();
        }

        public async Task<TelegramBotClient> GetBot()
        {
            if (_botClient == null)
            {
                _botClient = new TelegramBotClient(_config.Token);
                var hook = $"{_config.Url}/api/message/update";
                await _botClient.SetWebhookAsync(hook);
            }
            return _botClient;
        }
    }
}
