using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DictionaryBit.Data.Entities.Factories
{
    public static class UserFactory
    {
        public static User Create(Telegram.Bot.Types.User user)
        {
            var result = new User()
            {
                ChatId = user.Id,
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return result;
        }
    }
}
