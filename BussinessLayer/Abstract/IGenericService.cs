using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        void TCreate(T t);
        void TUpdate(T t);
        void TDelete(T t);
        public T TGetById(int id);
        public List<T> TGetList();
    }
}
