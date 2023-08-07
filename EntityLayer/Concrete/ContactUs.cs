using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ContactUs
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(14)]
        public double PhoneNumber { get; set; }
        [Required]
        public string Gmail { get; set; }

        public string Location { get; set; }

        public bool IsDeactive { get; set; }

    }
}
