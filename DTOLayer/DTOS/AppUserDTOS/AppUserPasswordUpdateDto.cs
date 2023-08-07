using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.DTOS.AppUserDTOS
{
	public class AppUserPasswordUpdateDto
	{
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
