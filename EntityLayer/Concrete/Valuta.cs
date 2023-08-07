using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Valuta
    {
        [Key]
        public int Id{ get; set; }

       public string Currency { get; set; }

        public List<CustomerAccount> CustomerAccounts { get; set; } 
    }
}
