using DataAccsessLayer.Abstract;
using DataAccsessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.EFrameworkCore
{
    public class EFCreditDetailRepository: GenericRepository<CreditDetail>, ICreditDetailDAL
    {
    }
}
