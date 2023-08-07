using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser:IdentityUser<int>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Surname { get; set; }

        public int ResetPasswordCode { get; set; }

        public DateTime ResetPasswordGenerationTime { get; set; }

       public int ResetPasswordCount { get; set; }

        public int ConfirmCode { get; set; }

		public int CodeGenerationAttempts { get; set; }

		public DateTime LastCodeGenerationTime { get; set; }

		public List<CustomerAccount> CustomerAccounts { get; set; }

        public int RemainingTimeForConfirmCode { get; set; } 

        public bool RuleAndContract { get; set; }  //Qaydalar və Məlumatlar
    }
}
