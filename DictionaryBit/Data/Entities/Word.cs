using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DictionaryBit.Data.Entities
{
    public sealed class Word: EntityBase
    {
        [Required]
        public string Foreign { get; set; }
        [Required]
        public string Native { get; set; }
        public string? Description { get; set; }
        [Required]
        public Guid DictionaryId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Dictionary Dictionary { get; set; }
        public User User { get; set; }
        public override string ToString()
        {
            return $"{Foreign}/{Native}/{Description}";
        }
    }
}
