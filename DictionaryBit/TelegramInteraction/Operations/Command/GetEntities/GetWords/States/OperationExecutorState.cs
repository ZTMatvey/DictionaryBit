using DictionaryBit.TelegramInteraction.Operations.Command.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.TelegramInteraction.Operations.Command.GetEntities.GetWords.States
{
    public class OperationExecutorState : IState
    {
        private readonly IServiceProvider _serviceProvider;
        public async Task ExecuteAsync()
        {
            //_serviceProvider.GetRequiredService<>();
        }
    }
}
