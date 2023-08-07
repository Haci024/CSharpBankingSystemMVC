using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EntityLayer.Concrete
{
    public class BankCards
    {
        public int ID { get; set; }

        public string Image { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Type { get; set; }
        public List<CardSkills> CardSkills { get; set; }

        public bool IsDeactive { get; set; }

    }
}
