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
    public class MonthlyPaymentManager : IMonthlyPaymentService
    {
        private readonly IMonthlyPaymentDAL _monthlyPaymentDAL;
        public MonthlyPaymentManager(IMonthlyPaymentDAL monthlyPaymentDAL)
        {
              _monthlyPaymentDAL = monthlyPaymentDAL;
        }
        public void TCreate(MonthlyPayment t)
        {
            _monthlyPaymentDAL.Create(t);
        }

        public void TDelete(MonthlyPayment t)
        {
            _monthlyPaymentDAL.Delete(t);
        }

        public MonthlyPayment TGetById(int id)
        {
            return _monthlyPaymentDAL.GetById(id);
        }

        public List<MonthlyPayment> TGetList()
        {
            return _monthlyPaymentDAL.GetList();
        }

        public MonthlyPayment GetMonthlyPayment(int Id)
        {
            return _monthlyPaymentDAL.PayCredit(Id);
        }

        public void TUpdate(MonthlyPayment t)
        {
            _monthlyPaymentDAL.Update(t);
        }

    }
}
