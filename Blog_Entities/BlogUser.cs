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
     [Table("BlogUsers")]
    public class BlogUser:BaseEntity
    {
        [DisplayName("Adı"), StringLength(25)]
        public string Name { get; set; }
        [DisplayName("Soyadı"), StringLength(25)]
        public string Surname { get; set; }
        [Required ,DisplayName("E-Posta"), StringLength(50)]
        public string Email { get; set; }
        [Required ,DisplayName("Kullanıcı Adı"), StringLength(25)]
        public string Username { get; set; }

        [DisplayName("Profil Fotoğrafı"), StringLength(50),ScaffoldColumn(false)]  // false dedik bu alan gelmeyecek viewe
        public string UserProfileImage { get; set; }

        [Required ,DisplayName("Şifre"), StringLength(100)]
        public string Password { get; set; }
        [DisplayName("Aktif Mi")]
        public bool IsActive { get; set; }
        [DisplayName("Admin Mi")]
        public bool IsAdmin { get; set; }
        [ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }   //eşsiz bir sayı oluşturuyor e-mail aktifliği için

        //ilişkiler

        public virtual List<Note> Notes { get; set; }  //bir kullancıın birden fazla notu olabilir
        public virtual List<Comment> Comments { get; set; } // bir kullanıcının birden fazla yorumu olabilr
        public virtual List<Liked> Likes { get; set; } //birden fazla begenisi olabilir.
    }
}
