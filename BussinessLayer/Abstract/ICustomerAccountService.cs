using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Abstract
{
    public interface ICustomerAccountService:IGenericService<CustomerAccount>
    {
       public  List<CustomerAccount> TMyCardsList(int id);
        public CustomerAccount TSender(int id,string valuta);
        public CustomerAccount TReceiver(int ReceiverId);
      


    }
}
