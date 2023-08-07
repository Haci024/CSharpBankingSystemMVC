using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CustomerAccount  //internal-public olaraq deyişirik 
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set;}=string.Empty;
        public decimal Balance { get; set; }
        public string BankBranch {get;set;}
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<CustomerActionProcess> Sender { get; set; }
        public List<CustomerActionProcess> Receiver { get; set; }// Pul növü {AZN,USD,TRL,EUR}
        public Valuta Valutas { get; set; }
        [ForeignKey(nameof(Valuta))]
        public int ValutaID { get; set; } 
       public string CCV { get; set; }
        public string ExploryMonth { get; set; } = "0";
        public string ExploryYear { get; set; } = "0";
		public int ActivateAttempts { get; set; } = 0;
        public int ActivateCode { get; set; } = 0;
        public bool Active { get; set; } = false;
        public List<Credits> Credits { get; set; }

    }
    
}


