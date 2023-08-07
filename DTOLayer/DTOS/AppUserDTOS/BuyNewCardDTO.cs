using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOS.AppUserDTOS
{
    public class BuyNewCardDTO
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string Currency { get; set; }

        public string BankBranch { get; set; }


    }
}
