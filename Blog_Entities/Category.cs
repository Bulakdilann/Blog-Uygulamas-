using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_Entities
{
    [Table("Categoryies")]
    public class Category:BaseEntity
    {
        [Required , DisplayName("Kategori Adı"),StringLength(50)]  //DisplayName("Başlık"), Tablelara başlık veriyor
        public string Title { get; set; }
        [StringLength(200), DisplayName("Açıklama")]
        public string Description { get; set; }

        // İlişkili alanları tanımlama

        public virtual List<Note> Notes{ get; set; }

		public Category()
		{
            Notes = new List<Note>();  //Fakedata ile data oluştururken hata vermesini önlemek için bu satuırı ekliyoruz.
		}


    }
}
