using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CreditDetail
    {
        public int Id { get; set; }
       public double MonthlyPayment { get; set; }
        public DateTime PaymentDate { get; set; }
       public decimal RemainingPayment { get; set; }
        public int CreditID { get; set; }
        public Credits Credits { get; set; }
    }
}
