using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Entities
{
    public sealed class Dictionary: EntityBase
    {
        public ICollection<Word> Words { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
