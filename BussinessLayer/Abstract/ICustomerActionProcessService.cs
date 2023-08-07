using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Abstract
{
    public interface ICustomerActionProcessService : IGenericService<CustomerActionProcess>
    {
        public Task<List<CustomerActionProcess>> TMyLastActionList(int id);
        public List<CustomerActionProcess> TMyCardListForOneCard(string valuta);
        public string TSelectValuta(string valuta);
        
    }
}
