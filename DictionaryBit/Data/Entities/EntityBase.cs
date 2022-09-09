using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBit.Data.Entities
{
    public class EntityBase
    {
        [Required]
        public Guid Id { get; set; }
    }
}
