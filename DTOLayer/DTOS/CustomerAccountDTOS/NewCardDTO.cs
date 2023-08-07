using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOS.CustomerAccountDTOS
{
    public class NewCardDTO
    {
        public decimal Balance { get; set; }
        public string BankBranch { get; set; }
        public int ValutaID { get; set; }
        public string CCV { get; set; }
        public string Currency { get; set; }
        public string ExploryMonth { get; set; }
        public string ExploryYear { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime LastLoginTime { get; set; }
        [Required(ErrorMessage ="Kart nömrəsi sistemdə olan kart nömrələri ilə eyni ola bilməz!")]
        public string AccountNumber { get;set; }

    }
}
