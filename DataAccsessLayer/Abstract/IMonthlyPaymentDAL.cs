using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Abstract
{
    public interface IMonthlyPaymentDAL:IGenericDAL<MonthlyPayment>
    {
        public  MonthlyPayment PayCredit(int id);
    }
}
