using DictionaryBit.TelegramInteraction;
using DictionaryBit.TelegramInteraction.Handlers;
using DictionaryBit.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DictionaryBit.Controllers
{
    [ApiController]
    [Route("api/message")]
    public sealed class TelegramBotController : ControllerBase
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TelegramBotClient _botClient;
        public TelegramBotController(
            IServiceProvider serviceProvider,
            RepositoryManager repositoryManager,
            IHttpContextAccessor contextAccessor,
            ITelegramBot telegramBot)
        {
            _serviceProvider = serviceProvider;
            _repositoryManager = repositoryManager;
            _contextAccessor = contextAccessor;
            _botClient = telegramBot.GetBot().Result;
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update(object update)
        {
            if (update == null)
                return Problem("Update couldn't be null");
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
            if (upd == null)
                return Problem("Update couldn't be null");

            TelegramUpdateHandler handler = null;
            if (upd.Message != null)
                handler = new TelegramUpdateMessageHandler(_repositoryManager, _serviceProvider, _contextAccessor);
            else if (upd.CallbackQuery != null)
                handler = new TelegramCallbackQueryHandler(_repositoryManager, _botClient, _serviceProvider, _contextAccessor);
            await handler?.HandleAsync(upd);
            return Ok();
        }
    }
}