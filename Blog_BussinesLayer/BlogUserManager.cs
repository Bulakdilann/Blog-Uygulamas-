using Blog_Common.Helpers;
using Blog_DataAccessLayer.EntityFrameworkSQL;
using Blog_Entities;
using Blog_Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BussinesLayer
{
	public class BlogUserManager :BaseManager<BlogUser>
	{
		//Repository<BlogUser> repository =new Repository<BlogUser>();

		public BussinesLayerResult<BlogUser> RegisterUser(RegisterViewModel model)
		{
			BlogUser user =Find(x=> x.Username==model.UserName || x.Email==model.Email);
			BussinesLayerResult<BlogUser> layerResult =new BussinesLayerResult<BlogUser>();

			if (user !=null)
			{
				// Kullanıcı adı ve e-mail: bunlardan hangisi sistemde var.
				if (user.Username==model.UserName)
				{
					layerResult.Errors.Add("Kullanıcı Adı Sistemde Kayıtlı");
				}

				if (user.Email==model.Email)
				{
					layerResult.Errors.Add("Girdiğiniz E-Posta sistemde kayıtlı");
				}

			}
			else
			{
				// Veritabanına bu kullanıcı kaydedilecek.
				int result=base.Insert(new BlogUser { 
					Name=model.Name,
					Surname=model.Surname,
					UserProfileImage="user-profile.jpg",
					Username=model.UserName, 
					Email = model.Email,
					Password=model.Password,
					IsActive=false,
					IsAdmin=false,
					CreatedDate=DateTime.Now,
					ModifiedDate=DateTime.Now,
					ModifiedUserName= model.UserName,
					ActivateGuid=Guid.NewGuid()
					});
				if (result>0)
				{
					layerResult.Result=Find(x=>x.Username==model.UserName && x.Email==model.Email);
					// kullanıcıya aktivasyon mailini göndermek için gerekli kodlar

					// Web.config dosyasının sitemizin rootunu eklemiştik. bu bilgiyi alıyoruz.
					string siteUrl=ConfigHelper.Get<string>("SiteRootUrl");  //siterooturl webconfigden geliyor

					//Gönderilen maildeki active linkini oluşturmak için activeUrl değişkenini tanımladık. Root/controller/action/guitParametresi link oluşturuldu.
					string activateUrl=$"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
					//mail de göndereceğimiz mesajın içeriğini oluşturdum.
					// mailhelper clasında ishtml=true oldugu için html taglarını kullanabiliyorum.
					string messageBody=$"Merhaba, hesabınızı aktifleştirmek için <a href='{activateUrl}' target='_blank'>tıklayınız.</a>";
					string subject="NA-203 Blog Hesap Aktifleştirme";

					// mailhelper'ın SendMail metoduna yukarıdaki parametreleri veriyorum. gönderilecek kişinin maili yukarıda sorguladıgım layerResult.Result içinden alıyorum.

					MailHelper.SendMail(messageBody, layerResult.Result.Email, subject);

				}


			}

			return layerResult;
		}

        public BussinesLayerResult<BlogUser> LoginUser(LoginViewModel model)
        {
            BussinesLayerResult<BlogUser> blResult = new BussinesLayerResult<BlogUser>();
			blResult.Result=Find(x=>x.Username ==model.UserName && x.Password==model.Password);

            if (blResult.Result !=null)
            {
                // kullanıcı sistemde kayıtlı ise geriye bu kullanıcı bilgileri gönderilecek.

                if (!blResult.Result.IsActive)
                {
					// kullanıcı kayıtlı ise aktif olup olmadıgı kontrol edilir. Aktif değilse error listesine aşagıdaki mesaj eklenir.
					blResult.Errors.Add("Hesabınız aktif değil. Lütfen e-postanızı kontrol ediniz.");
                }
            }
            else
            {
				blResult.Errors.Add("Kullanıcı adı ya da şifreniz hatalı. Ya da Kayıtlı kulanıcı değilsiniz.");
			}

			return blResult;
		}

        public BussinesLayerResult<BlogUser> UserActivate(Guid id)
		{
			BussinesLayerResult<BlogUser> blResult =new BussinesLayerResult<BlogUser>();
			blResult.Result=Find(x=> x.ActivateGuid==id);

			if (blResult.Result!=null)
			{
				if (blResult.Result.IsActive)
				{
					blResult.Errors.Add("kullanıcı zaten aktif edilmiştir");
				}
				else
				{
					blResult.Result.IsActive =true;
					Update(blResult.Result);
				}
			}
			else
			{
				blResult.Errors.Add("Aktifleştirilecek kullanıcı bulunamadı.");
			}
			return blResult;
		}

        public BussinesLayerResult<BlogUser> GetUserById(int id)
        {
            BussinesLayerResult<BlogUser> blResult = new BussinesLayerResult<BlogUser>();
			BlogUser user =Find (x=>x.Id ==id);
            if (user==null)
            {
				blResult.Errors.Add("Kullanıcı bulunamadı");
            }
            else
            {
				blResult.Result =user;
            }
			return blResult;

		}

        public BussinesLayerResult<BlogUser> DeleteUser(int id)
        {
			BussinesLayerResult<BlogUser> blResult = new BussinesLayerResult<BlogUser>();
			BlogUser user = Find(x => x.Id == id);
			if (user !=null)
			{
				// User'a ait Note'lar silinmeli 
				// User'a ait Likeler silinmeli
				// User'a ait Comment'ler silinmeli



                if (Delete(user)==0)
                {
					blResult.Errors.Add("kullanıcı silinemedi");
					return blResult;
                }
			}
			else
			{
				blResult.Errors.Add("kullanıcı bulunamadı");
			}
			return blResult;
		}

        public BussinesLayerResult<BlogUser> UpdateProfile(BlogUser userData)
        {
			BussinesLayerResult<BlogUser> blResult = new BussinesLayerResult<BlogUser>();
			BlogUser userDb = Find(x => x.Id != userData.Id && (x.Email == userData.Email || x.Username == userData.Username));

            if (userDb !=null && userDb.Id !=userData.Id)
            {
                if (userDb.Username== userData.Username)
                {
					blResult.Errors.Add("Girdiğiniz kullanıcı adı başka bir üyemiz tarafından kullanılmaktadır.Lütfen faklı bir kullanıcı adı giriniz");
                }

				if (userDb.Email == userData.Email)
				{
					blResult.Errors.Add("Girdiğiniz Email adresi sistemde kayıtlıdır.Lütfen faklı bir Email giriniz");

				}
				return blResult;
			}
			// eğer herhangi bir hata yoksa if bloguna girmeyecek ve buradan devam edecek. Bu satırdan sonra Update işlemlerini yapmam gerekecek.
			blResult.Result=Find(x=>x.Id ==userData.Id);
			blResult.Result.Name=userData.Name;
			blResult.Result.Surname=userData.Surname;
			blResult.Result.Email=userData.Email;
			blResult.Result.Username=userData.Username;
			blResult.Result.Password=userData.Password;

            // Fotograflar geldiyse bunun kontrolunu yapıyorum.

            if (string.IsNullOrEmpty(userData.UserProfileImage) == false)
            {
				blResult.Result.UserProfileImage = userData.UserProfileImage;
            }

            if (base.Update(blResult.Result)==0)
            {
				blResult.Errors.Add("Profil güncellenemedi.");
            }

			return blResult;
		}

		// miras olarak gelen bir metotodu ezmek istiyorsam ve geridönüş tipini değiştirmek istiyorsam
		//(basemanager içindeki insert isimli metodu ezmek istiyoruz. BaseManager içindeki Insert metodunu geri dönüş türü int türünde. fakat ben farklı bir türün geriye dönmesini istiyorsam(örrneğin string ya da BussinesLayerResult<BlogUser>), bu durumda aşagıdaki gibi new keyword'ünü kullanarak ezmek istediğim metodun geri dönüş tipini değiştirebilirim  ve artık insert metodu kullanmak istediginde buradaki metot kullanılacak. )
		// yukarıdaki RegisterUset metodunda BaseManager'daki Insert metodunu kullanmak istediğimizde oradaki Insert metodunun önüne base'i ekledik. Insert (BaseManager'daki insert metodu.)
		public new BussinesLayerResult<BlogUser> Insert(BlogUser data)
		{
			BlogUser user =Find(x=>x.Username== data.Username || x.Email==data.Email);
			BussinesLayerResult<BlogUser> layerResult =new BussinesLayerResult<BlogUser>();
			layerResult.Result= data;
			if (user !=null)
			{
				// bu durumda bir hata olmalı. yani e-mail ve kullanıcı adı baaşka bir kullanıcı tarafından kullanılıyor..kaydetme işlemi yapılmamalı ve geriye de kayıt giren kişiye uyarı mesajları gönderilmeli.

				if (user.Email==data.Email)
				{
					layerResult.Errors.Add("e-posta adresi kayıtlı");
				}

				if (user.Username == data.Username)
				{
					layerResult.Errors.Add("Kullanıcı adı kayıtlı");
				}
			}
			else
			{
				// username ve mail ile eşleşen kayıt yok ise veriyi ekleme işlemini yapmamız gerekir.
				layerResult.Result.UserProfileImage="user-profile.jpg";
				layerResult.Result.ActivateGuid=Guid.NewGuid();

				if (base.Insert(layerResult.Result) ==0 )
				{
					layerResult.Errors.Add("Yeni üye kaydedilirken bir hata oluştu.");
				}
			}

			return layerResult;
		}


		public new BussinesLayerResult<BlogUser> Update(BlogUser data)
		{
			BussinesLayerResult<BlogUser> layerResult= new BussinesLayerResult<BlogUser>();

			BlogUser dbUser =Find(x=>x.Id !=data.Id && (x.Email == data.Email || x.Username==data.Username));
			layerResult.Result=data;
			if (dbUser != null && dbUser.Id !=data.Id)
			{
				if (dbUser.Username == data.Username)
				{
					layerResult.Errors.Add("Girdiğiniz kullanıcı adı başka bir üyemiz tarafından kullanılıyor. Lütfen farklı bir kullanıcı adı girin");
				}

				if (dbUser.Email == data.Email)
				{
					layerResult.Errors.Add("Girdiğiniz E-posta başka bir üyemiz tarafından kullanılıyor. Lütfen farklı bir E-posta girin");
				}
				return layerResult;
			}
			// Eğer hata yoksa update işlemi ile ilgili işlemleri yapmalıyız.

			layerResult.Result =Find(x=>x.Id ==data.Id);
			layerResult.Result.Email= data.Email;
			layerResult.Result.Name= data.Name;
			layerResult.Result.Surname= data.Surname;
			layerResult.Result.Username= data.Username;
			layerResult.Result.Password= data.Password;
			layerResult.Result.IsActive= data.IsActive;
			layerResult.Result.IsAdmin= data.IsAdmin;

			if (base.Update(layerResult.Result) ==0)
			{
				layerResult.Errors.Add("Profil güncellenirken bir hata oluştu");
			}
		
			return layerResult;
		}

	}
}
