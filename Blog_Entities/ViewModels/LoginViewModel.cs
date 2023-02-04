using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog_Entities.ViewModels
{
	public class LoginViewModel  //kullanıcıdan aldıgım giriş bilgilerini burda tutuyorum
	{
		[DisplayName("Kullanıcı Adı"), Required(ErrorMessage ="{0} Boş Geçilemez"), StringLength(25,ErrorMessage ="{0} Alanı En Fazla 25 Karakter Olmalı")]
		public string UserName { get; set; }
		[DisplayName("Şifre"), Required(ErrorMessage = "Şifre Boş Geçilemez"),
		DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre Alanı En Fazla 25 Karakter Olmalı")]
		public string Password { get; set; }
	}
}