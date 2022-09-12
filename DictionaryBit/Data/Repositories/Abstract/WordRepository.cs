using DictionaryBit.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Repositories.Abstract
{
    public interface WordRepository
    {
        IEnumerable<Word> GetAllWordsByUserId(Guid userId);
        IEnumerable<Word> GetAllWordsByDictionaryId(Guid dictionaryId);
        Word GetById(Guid id);
        Task SaveAsync(Word entity);
        bool IsWordExist(Guid dictionaryId, string foreign);
    }
}
