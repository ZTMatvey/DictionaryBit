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
    public sealed class NotificationManager : IHostedService, IDisposable
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

        public async void Dispose()
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var users = _repositoryManager.UserRepository.GetAllUsers();
            foreach (var user in users)
                await _botClient.SendTextMessageAsync(user.ChatId, "<<Бот запущен>>");
            _timer?.Dispose();
            _timer = new Timer(s => SendNotifications(), null, TimeSpan.Zero, TimeSpan.FromMinutes(_config.NotificationFrequency));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var users = _repositoryManager.UserRepository.GetAllUsers();
            foreach (var user in users)
                await _botClient.SendTextMessageAsync(user.ChatId, "<<Бот остановлен>>");
            _timer?.Change(Timeout.Infinite, 0);
        }
    }
}
