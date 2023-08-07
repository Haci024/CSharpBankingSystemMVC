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
    public class CreditDetailManager : ICreditDetailService
    {
        private readonly ICreditDetailDAL _CreditDetailDAL;
        public CreditDetailManager(ICreditDetailDAL creditDetailDAL)
        {
            _CreditDetailDAL = creditDetailDAL;
        }
        public void TCreate(CreditDetail t)
        {
            _CreditDetailDAL.Create(t);
        }
        public void TDelete(CreditDetail t)
        {
            _CreditDetailDAL.Delete(t);
        }
        public CreditDetail TGetById(int id)
        {
            return _CreditDetailDAL.GetById(id);
        }
        public List<CreditDetail> TGetList()
        {
            return _CreditDetailDAL.GetList();
        }
        public void TUpdate(CreditDetail t)
        {
            _CreditDetailDAL.Update(t);
        }
    }
}
