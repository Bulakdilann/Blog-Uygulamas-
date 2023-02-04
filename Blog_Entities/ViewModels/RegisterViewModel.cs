using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog_Entities.ViewModels
{
	public class RegisterViewModel
	{
		[DisplayName("Adınız"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalı.")]
		public string Name { get; set; }

		[DisplayName("Soyadınız"), Required(ErrorMessage = "{0} boş geçilemez."), StringLength(25, ErrorMessage = "{0} alanı en fazla 25 karakter olmalı.")]
		public string Surname { get; set; }

		[DisplayName("E-posta"), Required(ErrorMessage = "{0} Boş Geçilemez"), StringLength(50, ErrorMessage = "{0} Alanı En Fazla 50 Karakter Olmalı"), EmailAddress(ErrorMessage ="{0} alanı geçerli bir e-posta giriniz")]
		public string Email { get; set; }

		[DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Boş Geçilemez"), StringLength(25, ErrorMessage = "{0} Alanı En Fazla 25 Karakter Olmalı")]
		public string UserName { get; set; }

		[DisplayName("Şifre"), Required(ErrorMessage = "Şifre Boş Geçilemez"),
		DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre Alanı En Fazla 25 Karakter Olmalı")]
		public string Password { get; set; }

		[DisplayName("Şifre"), Required(ErrorMessage = "Şifre Tekrar Alanı Boş Geçilemez"),
		DataType(DataType.Password), StringLength(25, ErrorMessage = "Şifre Tekrar Alanı En Fazla 25 Karakter Olmalı"), Compare("Password",ErrorMessage ="Girilen şifreler eşleşmiyor.")]
		public string RePassword { get; set; }
	}
}