using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CustomerActionProcess
    {
        public int Id { get; set; }

        public string ProcessType { get; set; }
            
        public decimal Amount { get; set; }
            
        public DateTime ProcessDate { get; set; }

        public int? SenderID { get; set; }

        public CustomerAccount SendAction { get; set; }
        public CustomerAccount ReceiveAction { get; set; }
        
        public int? ReceiverID { get; set; }

        public decimal? fees { get; set; }

    }
}
