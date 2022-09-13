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
    public class User: EntityBase
    {
        [MaxLength(50)]
        public string? UserName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        public long ChatId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Dictionary>? Dictionaries { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Word>? Words { get; set; }
    }
}
