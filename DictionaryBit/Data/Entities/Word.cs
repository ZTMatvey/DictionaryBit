﻿using System.ComponentModel.DataAnnotations;

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
        public Dictionary Dictionary { get; set; }
        public Guid DictionaryId { get; set; }
    }
}
