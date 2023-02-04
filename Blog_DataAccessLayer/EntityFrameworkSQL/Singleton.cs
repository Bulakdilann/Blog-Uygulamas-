using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
   public class Singleton
    {
        protected static BlogContext _context;
        private static object _lock = new object();
        protected Singleton()   
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (_context == null)
            {
                // Bazı uygulamalarda (multitrade uygulamalarda), aynı anda 2 tane istek if bloguna girebilir. 
                // bu gibi durumlaarı kontrol etmek için, lock ile kilitleme yapılır. 
                // yani lock aynı anda 2 tane istekin ya da trade'in çalıstırılmayacagını söyler.
                lock (_lock)
                {
                    if (_context == null)
                    {
                        _context = new BlogContext();
                    }
                    
                }
                
            }
           // return _context;
            
        }
    }
}
