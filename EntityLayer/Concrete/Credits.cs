using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Credits
    {
        public int Id { get; set; }

        public double CreditAmount { get; set; }

        public double Period { get; set; }
        
        public bool IsActive { get; set; }

        public double Percent { get; set; }

        public bool IsCompleted { get; set; } = false;

       public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public CreditDetail CreditDetail { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public int CustomerAccountID { get; set; }

        public double TotalPayment { get; set; }

        public List<MonthlyPayment> MonthlyPayment  { get; set; }
       
        public string Document { get; set; }
        [NotMapped]
        public IFormFile Fax { get; set; }
    }
}
