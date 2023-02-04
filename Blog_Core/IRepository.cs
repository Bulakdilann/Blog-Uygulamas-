using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Core
{
   public interface IRepository<T>
    {
        // Birden fazla kayıt almak için
        List<T> List();
        List<T> List(Expression<Func<T, bool>> filter);

        // tek bir kayıt geriye dönsün istiyorsak 
        IQueryable<T> ListQueryable();
        IQueryable<T> ListQueryable(Expression<Func<T,bool>> filter);

        T GetById(int id);   // İd e göre arama
        T Find(Expression<Func<T, bool>> filter);
        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Save();

    }
}
