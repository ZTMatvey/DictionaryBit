using DictionaryBit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Repositories.Abstract
{
    public interface UserRepository
    {
        Task SaveAsync(User entity);
        User GetById(Guid id);
        IEnumerable<User> GetAllUsers();
        User GetByChatId(long id);
        bool HasDictionary(Guid userId, Guid dictionaryId);
    }
}
