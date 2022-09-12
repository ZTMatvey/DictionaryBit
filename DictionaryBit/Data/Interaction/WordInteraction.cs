using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Interaction
{
    public sealed class WordInteraction
    {
        private readonly RepositoryManager _repositoryManager;

        public WordInteraction(RepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<SaveWordResult> SaveAsync(Word word, User user)
        {
            if (user == null || word == null)
                throw new ArgumentNullException();
            var userHasDictionary = _repositoryManager.UserRepository.HasDictionary(user.Id, word.DictionaryId);
            if (!userHasDictionary)
                return SaveWordResult.UserHasNoDictionary;
            try
            {
                await _repositoryManager.WordRepository.SaveAsync(word);
                return SaveWordResult.Ok;
            }
            catch (Exception)
            {
                return SaveWordResult.NotOk;
            }
        }
        public bool CheckWordForExist(Guid dictionaryId, string foreign)
        {
            var result = _repositoryManager.WordRepository.IsWordExist(dictionaryId, foreign);
            return result;

        }
    }
}
