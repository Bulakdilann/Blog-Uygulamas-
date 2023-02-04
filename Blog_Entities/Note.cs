using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Entities
{
     [Table("Notes")]
    public class Note:BaseEntity
    {
        [Required ,StringLength(50)]
      
        public string Title { get; set; }
        [Required ,StringLength(2000)]
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int CategoryId { get; set; }

        //ilişkiler

        public virtual Category Category { get; set; }   //categoriıd demiyoyorum cunkü zaten bu tanımı yapınca databasede geliyor
        public virtual BlogUser Owner { get; set; }
        public virtual  List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

		public Note()
		{
            Comments =new List<Comment>();   //fake data hatasının önune geçmek için yazdım
            Likes =new List<Liked>();
		}
        


    }
}
