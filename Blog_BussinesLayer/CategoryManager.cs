using Blog_DataAccessLayer.EntityFrameworkSQL;
using Blog_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BussinesLayer
{
  public class CategoryManager : BaseManager<Category>
	{
        //private Repository<Category> repository= new Repository<Category>();

        //public List<Category> GetCategories()
        //{
        //	return repository.List();
        //}

        //public Category GetCategoryByID(int id)
        //{
        //	return repository.Find(x=>x.Id==id);
        //}

        // Delete metodunu normalde BaseManager sınıfındaki metodu kullanıyor. Biz Basemanager sınıfınını abstrac olarak işaretledik. içindeki metodlarıda virtual olarak işaretledik. bu sınıf(CategoryManager) Basemanager sınıfını miras aldıgı için ve yukarı saydıgımız özelliklerden dolayı, orada tanımanan metotları burada ezebiliriz.
        public override int Delete(Category category)
        {
            // bir kategorinin silinmesi için  ilişkili olan kayıtlarında silinmesi gerekiyor.
            // (Note, Comment, Liked)

            NoteManager noteManager= new NoteManager();
            CommentManager commentManager = new CommentManager();
            LikedManager likedManager = new LikedManager();

            foreach (var note in category.Notes.ToList())
            {
                // Bu note'a ait comment'leri de silmem gerekli
                foreach (var comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }
                // Bu note'a ait Like'ları silmem gerekli
                foreach (var like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }
                noteManager.Delete(note);
            }


            
            return base.Delete(category); // Bu satır, bu metodun içindeki kodların yanında bASEManager içindeki Delete metodununda çalışacagı anlamına gelir.
        }
    }
}
