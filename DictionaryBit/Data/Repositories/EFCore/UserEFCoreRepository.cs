using DictionaryBit.Data.Context;
using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Repositories.EFCore
{
    public sealed class UserEFCoreRepository : UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserEFCoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync(User entity)
        {
            if (entity.Id == default)
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            else
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }
        public User GetById(Guid id)
        {
            var entity = _context.Users.FirstOrDefault(x=> x.Id == id);
            return entity;
        }
        public User GetByChatId(long id)
        {
            var entity = _context.Users.FirstOrDefault(x=> x.ChatId == id);
            return entity;
        }
        public bool HasDictionary(Guid userId, Guid dictionaryId)
        {
            var entity = _context.Dictionaries.FirstOrDefault(x => x.UserId == userId && x.Id == dictionaryId);
            return entity != null;
        }
    }
}
