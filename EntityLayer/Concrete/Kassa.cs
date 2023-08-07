using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
	public class Kassa
	{
        public int Id { get; set; }

        public decimal Balance { get; set; }

        
    }
}
