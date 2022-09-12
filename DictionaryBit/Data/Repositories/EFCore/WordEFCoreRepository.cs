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
    public sealed class WordEFCoreRepository : WordRepository
    {
        private readonly ApplicationDbContext _context;

        public WordEFCoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync(Word entity)
        {
            if (entity.Id == default)
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            else
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public Word GetById(Guid id)
        {
            var entity = _context.Words.FirstOrDefault(x => x.Id == id);
            return entity;
        }
        public IEnumerable<Word> GetAllWordsByDictionaryId(Guid dictionaryId)
        {
            var result = _context.Words.Where(x => x.DictionaryId == dictionaryId);
            return result;
        }
        public bool WordExist(Guid dictionaryId, string foreign)
        {
            var lowerForeign = foreign.ToLower();
            var words = GetAllWordsByDictionaryId(dictionaryId);
            var word = words?.FirstOrDefault(x => x.Foreign.ToLower() == lowerForeign);
            var isExist = word != null;
            return isExist;
        }
    }
}
