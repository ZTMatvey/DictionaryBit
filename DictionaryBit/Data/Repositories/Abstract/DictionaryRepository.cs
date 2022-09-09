using DictionaryBit.Data.Entities;

namespace DictionaryBit.Data.Repositories.Abstract
{
    public interface DictionaryRepository
    {
        IEnumerable<Dictionary> GetAllDictionariesByUserId(Guid userId);
        Dictionary GetById(Guid id);
        Task SaveAsync(Dictionary entity);
    }
}
