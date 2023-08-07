using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MonthlyPayment
    {
        public int Id { get; set; }
       
        public decimal Payment { get; set; }
       
        public DateTime PaymentDate { get; set; }

        public bool IsPay { get; set; } = false;

        public Credits Credit { get; set; }

        public int CreditID { get; set; }

        public DateTime ProcessDate { get; set; }

        public decimal PayMoney { get; set; } = 0;

        public string Document { get; set; }
        [NotMapped]
        public IFormFile Fax { get; set; }



    }
}
