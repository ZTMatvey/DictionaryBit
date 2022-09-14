using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Entities
{
    public sealed class Dictionary: EntityBase
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Word> Words { get; set; }
        [Required]
        public string Name { get; set; }
        public User User { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
