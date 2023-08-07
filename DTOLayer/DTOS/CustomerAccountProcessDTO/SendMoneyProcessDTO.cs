using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOS.CustomerAccountProcessDTO
{
	public class SendMoneyProcessDTO
	{
		public int Id { get; set; }

		public string ProcessType { get; set; }

		public decimal Amount { get; set; }

		public string ReceiverCardNumber { get; set; }

		public DateTime ProcessDate { get; set; }

		public int SenderID { get; set; }

		public int ReceiverID { get; set; }

		public decimal fess { get; set; }

		
        public string SenderCard { get; set; }


    }
}
