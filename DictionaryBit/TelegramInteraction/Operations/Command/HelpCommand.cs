using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DictionaryBit.TelegramInteraction.Operations.Command
{
    public class HelpCommand : CommandBase
    {
        public override string CommandName => CommandNames.Help;
        public HelpCommand(RepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ITelegramBot telegramBot) : base(repositoryManager, httpContextAccessor, telegramBot)
        {}
        protected override async Task<string> ExecuteAndGetNextOperationAsync(User user, string content)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine($"*Use dictionary*");
            resultBuilder.AppendLine("Включает активный словарь. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.UseDictionary}— получить список из ваших словарей для выбора");
            resultBuilder.AppendLine($"{CommandNames.UseDictionary} <dictionaryName>— установить словарь <dictionaryName>");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Loss dictionary*");
            resultBuilder.AppendLine("Выключить активный словарь. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.LossDictionary}");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Add dictionary*");
            resultBuilder.AppendLine("Добавить словарь в коллекцию. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.AddDictionary}— начать операцию добавления");
            resultBuilder.AppendLine($"{CommandNames.AddDictionary} <dictionaryName>— добавить словарь <dictionaryName>");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Remove dictionary*");
            resultBuilder.AppendLine("Удалить словарь из коллекции. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.RemoveDictionary}— начать операцию удаления");
            resultBuilder.AppendLine($"{CommandNames.RemoveDictionary} <dictionaryName>— удалить словарь <dictionaryName>");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Get dictionaries*");
            resultBuilder.AppendLine("Получить коллекцию словарей. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.GetDictionaries}");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Get words*");
            resultBuilder.AppendLine("Получить слова. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.GetWords}— начать операцию получения");
            resultBuilder.AppendLine($"{CommandNames.GetWords} from <dictionaryName>— получить слова из словаря <dictionaryName>");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Add word*");
            resultBuilder.AppendLine("Добавить слово. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.AddWord}— начать операцию добавления");
            resultBuilder.AppendLine($"{CommandNames.AddWord} f/n/d— задать слову параметры foreign, native, description соответственно");
            resultBuilder.AppendLine($"{CommandNames.AddWord} f/n/d to <dictionaryName>— добавить слово с параметрами foreign, native, description в словарь <dictionaryName>");
            resultBuilder.AppendLine();
            resultBuilder.AppendLine($"*Random word*");
            resultBuilder.AppendLine("Получить случайное слово. Вариации:");
            resultBuilder.AppendLine($"{CommandNames.GetDictionaries}— из всех словарей");
            resultBuilder.AppendLine($"{CommandNames.GetDictionaries} from <dictionaryName>— из словаря <dictionaryName>");

            await botClient.SendTextMessageAsync(user.ChatId, resultBuilder.ToString(), parseMode:Telegram.Bot.Types.Enums.ParseMode.Markdown);
            
            return string.Empty;
        }
    }
}
