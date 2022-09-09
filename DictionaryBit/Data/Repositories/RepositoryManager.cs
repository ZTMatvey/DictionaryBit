using DictionaryBit.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Repositories
{
    public sealed class RepositoryManager
    {
        public readonly UserRepository UserRepository;
        public readonly DictionaryRepository DictionaryRepository;
        public readonly WordRepository WordRepository;

        public RepositoryManager(UserRepository userRepository, DictionaryRepository dictionaryRepository, WordRepository wordRepository)
        {
            UserRepository = userRepository;
            DictionaryRepository = dictionaryRepository;
            WordRepository = wordRepository;
        }
    }
}
