using Blog_Core;
using Blog_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
    public class Repository<T> :Singleton, IRepository<T> where T : class
    {
        //private BlogContext _context;
        private DbSet<T> _object;
        public Repository()
        {
           // _context=Singleton.CreateContext();
            _object= _context.Set<T>();  //CONTEXT İN içinde ilgili değeri alacagım _context=blogcontextdeki değerler
        }

        public int Delete(T entity)
        {
            _object.Remove(entity);
            return Save();  // ne zaman savechange calısırsa o zaman veri tabanından siliyor
        }

        public T Find(Expression<Func<T, bool>> filter)  // buldugu ilk kaydı bize geri döndürecek
        {
            // Verilen Kritere göre veritabanında bulunan ilk kaydı getirir. (x=> x.Id ==KategoriId)
            return _object.FirstOrDefault(filter);
        }

        public T GetById(int id) //geriye sadece 1 değer dondurecek
        {
           T result= _object.Find(id);
           return result;
        }

        public int Insert(T entity)
        {
            _object.Add(entity);
            if (_object is BaseEntity)
            {
                BaseEntity entity1 = _object as BaseEntity;
                entity1.ModifiedDate = DateTime.Now;
                entity1.CreatedDate=DateTime.Now;
                entity1.ModifiedUserName = "system";
                //TODO: buraya işlem yapan kullanıcının username i gelmeli.
            }
            return Save();
            
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();  // sorguyu hemen atıyor
        }

      

        public IQueryable<T> ListQueryable()
        {
            return _object.AsQueryable<T>();
        }

        public IQueryable<T> ListQueryable(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public int Update(T entity)
        {
            if (_object is BaseEntity)
            {
                BaseEntity entity1=_object as BaseEntity;
                entity1.ModifiedDate=DateTime.Now;
                entity1.ModifiedUserName="system";
            }
            return Save();
        }
    }
}
