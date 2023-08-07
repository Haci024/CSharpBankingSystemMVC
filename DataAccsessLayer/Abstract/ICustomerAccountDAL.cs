using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Abstract
{
    public interface ICustomerAccountDAL:IGenericDAL<CustomerAccount>
    {
        public List<CustomerAccount> MyCardsList(int id);
        public CustomerAccount Sender(int id,string valuta);
        public CustomerAccount Receiver(int ReceiverId);
       
       
    }
}
