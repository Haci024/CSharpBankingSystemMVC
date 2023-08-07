using BussinessLayer.Abstract;
using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Concrete
{
    public class CustomerAccountManager : ICustomerAccountService
    {
        private readonly ICustomerAccountDAL _customerAccountDAL;
        public CustomerAccountManager(ICustomerAccountDAL customerAccountDAL)
        {
            _customerAccountDAL = customerAccountDAL;
            
        }
        public void TCreate(CustomerAccount t)
        {
            _customerAccountDAL.Create(t);
           
        }

        public void TDelete(CustomerAccount t)
        {
            _customerAccountDAL.Delete(t);
        }

        public CustomerAccount TGetById(int id)
        {
           return _customerAccountDAL.GetById(id);
        }

        public List<CustomerAccount> TGetList()
        {
          
            return _customerAccountDAL.GetList();
        }

     

        public List<CustomerAccount> TMyCardsList(int id)
        {
            return _customerAccountDAL.MyCardsList(id);
        }

        public CustomerAccount TReceiver(int ReceiverId)
        {
            return _customerAccountDAL.Receiver(ReceiverId);
        }

        public CustomerAccount TSender(int id, string valuta)
        {
            return _customerAccountDAL.Sender(id,valuta);
        }

        public void TUpdate(CustomerAccount t)
        {
           _customerAccountDAL.Update(t);
        }
    }
}
