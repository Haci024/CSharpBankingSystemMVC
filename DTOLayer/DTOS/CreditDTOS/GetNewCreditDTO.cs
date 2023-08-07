using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOS.CreditDTOS
{
    public class GetNewCreditDTO
    {

        public int CreditAmount { get; set; }


        public double Period { get; set; }

        public double Percent { get; set; }

        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }//Razilaşma fax

        public double TotalPayment { get; set; }

        public IFormFile Fax { get; set; }



    }
}
