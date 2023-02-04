using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Entities
{
     [Table("Comments")]
   public class Comment:BaseEntity
    {
        [Required ,StringLength(300)]
        public string Text { get; set; }

        //ilişkiler

        public virtual Note Note { get; set; }  // Hangi yazıya yorum yapıldı
        public virtual BlogUser Owner { get; set; }  //yorumu kim yaptı bilgisini tutacak
    }
}
