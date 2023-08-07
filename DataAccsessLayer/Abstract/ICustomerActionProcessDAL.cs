using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Abstract
{
    public interface ICustomerActionProcessDAL:IGenericDAL<CustomerActionProcess>
    {
        public  Task<List<CustomerActionProcess>> MyLastActionList(int id);
        public List<CustomerActionProcess> MyCardListForOneCard(string valuta);
        public string SelectValuta(string valuta);




    }
}
