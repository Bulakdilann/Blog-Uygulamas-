using Blog_DataAccessLayer.EntityFrameworkSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BussinesLayer
{
   public class Test
    {
        public Test()  //constaktır calıştıgı için hemen geliyor
        {
            BlogContext db=new BlogContext();
            db.BlogUsers.ToList();

          //  db.Database.CreateIfNotExists();  // DATAbase yoksa databasei yarat diyor
        }
    }
}
