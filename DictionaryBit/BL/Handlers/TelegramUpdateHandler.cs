using DictionaryBit.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DictionaryBit.BL.Handlers
{
    public abstract class TelegramUpdateHandler
    {
        protected readonly RepositoryManager _repositoryManager;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IHttpContextAccessor _httpContext;
        public TelegramUpdateHandler(RepositoryManager repositoryManager, IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
        {
            _repositoryManager = repositoryManager;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
        }
        public abstract Task HandleAsync(Update update);
    }
}
