using DictionaryBit.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DictionaryBit.BL
{
    public sealed class PollingTelegramBot: ITelegramBot
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly Config _config;
        private TelegramBotClient _botClient;
        public PollingTelegramBot()
        {
            _config = Config.Instance;
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
                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = new UpdateType[]
                    {
                        UpdateType.Message,
                        UpdateType.CallbackQuery
                    }, 
                };
                _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
            }
            return _botClient;
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellation)
        {
            Console.WriteLine($"Exception: {exception.Message}");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellation)
        {
            var jsonUpdate = JsonConvert.SerializeObject(update);
            var content = new StringContent(jsonUpdate, Encoding.UTF8, "application/json");
            await _client.PostAsync($"{_config.Url}/api/message/update", content);
        }
    }
}
