using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CardSkills
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Ability { get; set; }
        [Required]
        public int BankCardID { get; set; }

        public BankCards BankCard { get; set; }

        public bool Active { get; set; }
    }
}
