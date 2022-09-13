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
    public sealed class DictionaryEFCoreRepository : DictionaryRepository
    {
        private readonly ApplicationDbContext _context;

        public DictionaryEFCoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync(Dictionary entity)
        {
            if (entity.Id == default)
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            else
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public Dictionary GetById(Guid id)
        {
            var entity = _context.Dictionaries.FirstOrDefault(x => x.Id == id);
            return entity;
        }
        public Dictionary? GetByName(string name, Guid userId)
        {
            return _context.Dictionaries.FirstOrDefault(x=> x.Name == name && x.UserId == userId);
        }
        public IEnumerable<Dictionary> GetAllDictionariesByUserId(Guid userId)
        {
            var result = _context.Dictionaries.Where(x => x.UserId == userId);
            return result;
        }
        public Dictionary CheckAndGetById(Guid dictionaryId, Guid userId)
        {
            if (dictionaryId == default)
                return default;
            var dictionary = GetById(dictionaryId);
            if (dictionary.UserId != userId)
                return default;
            return dictionary;
        }
    }
}
