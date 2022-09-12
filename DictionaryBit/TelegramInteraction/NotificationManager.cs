using DictionaryBit.Data.Context;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction
{
    public sealed class NotificationManager: IHostedService, IDisposable
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly TelegramBotClient _botClient;
        private readonly Config _config;
        private Timer _timer;
        public NotificationManager(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            _repositoryManager = scope.ServiceProvider.GetRequiredService<RepositoryManager>();
            _botClient = scope.ServiceProvider.GetRequiredService<ITelegramBot>().GetBot().Result;
            _config = scope.ServiceProvider.GetRequiredService<Config>();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task SendNotifications()
        {
            var users = _repositoryManager.UserRepository.GetAllUsers();
            var random = new Random();
            foreach (var user in users)
            {
                var words = _repositoryManager.WordRepository.GetAllWordsByUserId(user.Id);
                var count = words.Count();
                if (count == 0)
                    continue;
                var skip = random.Next(0, count);
                var wordToNoti = words.Skip(skip).First();
                await _botClient.SendTextMessageAsync(user.ChatId, wordToNoti.ToString());
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _timer = new Timer(s=> SendNotifications(), null, TimeSpan.Zero, TimeSpan.FromMinutes(_config.NotificationFrequency));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
