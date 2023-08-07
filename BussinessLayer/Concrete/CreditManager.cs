using BussinessLayer.Abstract;
using DataAccsessLayer.Abstract;
using DTOLayer.DTOS.CustomerAccountProcessDTO;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Concrete
{
    public class CreditManager : ICreditService
    {
        private readonly ICreditDAL _CreditDAL;
        public CreditManager(ICreditDAL creditDAL)
        {
            _CreditDAL = creditDAL;

        }
      
        public void TCreate(Credits t)
        {
            _CreditDAL.Create(t);

        }

        public void TDelete(Credits t)
        {
            _CreditDAL.Delete(t);
        }

        public Credits TGetById(int id)
        {
            return _CreditDAL.GetById(id);
        }

        public List<Credits> TGetList()
        {

            return _CreditDAL.GetList();
        }

        public void TUpdate(Credits t)
        {
            _CreditDAL.Update(t);
        }
        public List<Credits> ICredits()
        {
            return _CreditDAL.ICredits();
                
        }
    }
}
