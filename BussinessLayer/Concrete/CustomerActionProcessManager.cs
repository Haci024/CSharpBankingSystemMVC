using BussinessLayer.Abstract;
using DataAccsessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Concrete
{
    public class CustomerActionProcessManager : ICustomerActionProcessService
    {
        private readonly ICustomerActionProcessDAL _db;

        public CustomerActionProcessManager(ICustomerActionProcessDAL db)
        {
            _db= db;    
        }
        public void TCreate(CustomerActionProcess t)
        {
            _db.Create(t);
        }

        public void TDelete(CustomerActionProcess t)
        {
           _db.Delete(t);
        }
        public List<CustomerActionProcess> TMyCardListForOneCard(string valuta)
        {
            return _db.MyCardListForOneCard(valuta);
        }
        public CustomerActionProcess TGetById(int id)
        {
            return _db.GetById(id);
        }

        public List<CustomerActionProcess> TGetList()
        {
            return _db.GetList();
        }

        public async Task<List<CustomerActionProcess>> TMyLastActionList(int Id)
        {
            return await _db.MyLastActionList(Id);
        }

        public void TUpdate(CustomerActionProcess t)
        {
            _db.Update(t);  
        }

        public string TSelectValuta(string valuta)
        {
            return _db.SelectValuta(valuta);
        }
    }
}
