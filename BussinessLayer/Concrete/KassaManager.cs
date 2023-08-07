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
	
	public class KassaManager : IKassaService
	{

		private readonly IKassaDAL kassaDAL;
        public KassaManager(IKassaDAL IkassaDAL)
        {
            kassaDAL = IkassaDAL;
        }
        public void TCreate(Kassa t)
		{
			kassaDAL.Create(t);
		}

		public void TDelete(Kassa t)
		{
			kassaDAL.Delete(t);
		}

		public Kassa TGetById(int id)
		{
			return kassaDAL.GetById(id);
		}

		public List<Kassa> TGetList()
		{
			return kassaDAL.GetList();
		}

		public void TUpdate(Kassa t)
		{
			 kassaDAL.Update(t);
		}
	}
}
