using Blog_BussinesLayer;
using Blog_Entities;
using Blog_Entities.ViewModels;
using Blog_WebUI.Filter;
using Blog_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Controllers
{
    [HandleException]
    public class HomeController : Controller
    {
        // GET: Home
        Test test= new Test();  //TESTİ KAPAT SONRA İŞİN KALMADI

      private  NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager=new CategoryManager();
        private BlogUserManager blogUserManager=new BlogUserManager();
        public ActionResult Index()
        {
            //int a=0;
            //int b=5;
            //int c=b/a;
            // [HandleException] kullandıgımız zaman, normalde yukarıda bir hata verrmesi gerekir.. ve bizi HandleException class'ı içinde tanımladıgımız Controller ve Actıona yönlendirip,hata mesajıni ekrana yazması gerekiyor. Projeyi yukarıda IIS Express ile çalıştırdıgımızda development modda oldugu için o sayfaya yönlendirmeyecek ve direk hatanın bulunduğu sayfa ve satıra gidilecektir. Bu sayfayı test edebilmek için Index.cshtml sayfasında sagtuş=>View in Browser ile çalıştırdığımızda bu Runtime hatası HandleException classı ile yakalanacak ve HasError View'i görüntülenecek.

            return View(noteManager.ListQueryable().Where(x=> x.IsDraft==false).OrderByDescending(x=> x.ModifiedDate).ToList());
        }

        public ActionResult SonYazılar()
        {
            return View("Index", noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedDate).Take(10).ToList());
        }
        public ActionResult MostLiked()
		{
            //en begenilenler
           
            return View ("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

       

        public ActionResult SelectCategory(int id)
		{
            
            Category category = categoryManager.Find(x=> x.Id ==id);

            return View("Index",category.Notes.OrderByDescending(x=> x.ModifiedDate).ToList());
		}
        [HttpGet]
        public ActionResult Login()   //login sayfasını açacagım yer  //default olarak httpget olur
		{
            return View();
		}
        [HttpPost]
        public ActionResult Login(LoginViewModel model)  //değer alacagız loginviewmodel den
        {
            // Giriş kontrolü
            // anasayfa yönlendirme
            // kullanıcı bilgilerini Sesion'a aktarma.
			if (ModelState.IsValid) // lognviewmodel de girdiğim (kuralların) degerlerinin kontrolunu yapıyor.
			{
                
                BussinesLayerResult<BlogUser> blResult = blogUserManager.LoginUser(model);
                // Eger hata varsa blresult icinde error liste eklenmiş olacak. bunun kontrolunu yapıyorum.
                if (blResult.Errors.Count>0)
                {
                    // hata mesajlarını modelstade ekliyorum.. hatalar ekranda görünecek
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("",x));
                    return View(model);
                }
                // session' da kullanıcının bilgilerini saklıyorum
                //Session["login"]= blResult.Result;
                CurrentSession.Set<BlogUser>("login",blResult.Result);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Logout() //çıkıs
        {
            //Session.Clear();
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Register()   //kayıt sayfası açacagım yer  //default olarak httpget olur
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)  //değer alacagız loginviewmodel den
        {
            if (ModelState.IsValid) 
            {
                
                BussinesLayerResult<BlogUser> blResult = blogUserManager.RegisterUser(model);
				if (blResult.Errors.Count >0)
				{
                    //  kod buraya girdiyse bu durumda email ya da kullanıcı adı kullanılıyor demektir. bu hata mesajlarını da ekrana yazdırmam gerekiyor ve kullanıcıyı uyarmama gerekiyor.
                    blResult.Errors.ForEach(x=> ModelState.AddModelError("",x)); // x errorlistin içindeki herbir değeri temsil ediyor.
                    // AddModelError içindeki hata mesajlarını ekranda görebiliyorum. ama BusinessLayerResult içindeki Error listen gelen
                    // hata mesajlarını göremiyorum. yukardaki kod ile Error Listdeki mesajları AddModelError içine eklemiş oluyorum.
                    return View(model);
				}

               return RedirectToAction("RegisterSuccess");
            }
            return View(model);
        }

        public ActionResult RegisterSuccess()   //kullanıcı başarılı bir şekilde kaydoldu actionu
		{
            return View();
		}

        public ActionResult UserActivate(Guid id)
		{
            // Maile gelen aktivasyon linkine tıklandıgında çalışacak olan Action burasıdır.

            
            BussinesLayerResult<BlogUser> blResult = blogUserManager.UserActivate(id);

			if (blResult.Errors.Count>0)
			{
                TempData["errors"] =blResult.Errors;
                return RedirectToAction("ActivateUserCancel");
			}
            return RedirectToAction("ActivateUserOk");
		}

        public ActionResult ActivateUserOk()
        {
            return View();
        }

        public ActionResult ActivateUserCancel()
		{
            List<string> errors=null;
			if (TempData["errors"] !=null)
			{
                errors =TempData["errors"] as List<string>;
			}
            return View(errors);  
		}
        [Auth]
        public ActionResult ShowProfile()
        {
           // BlogUser currentUser= Session["login"] as BlogUser;
           BlogUser currentUser =CurrentSession.User;
           
            BussinesLayerResult<BlogUser> blResult = blogUserManager.GetUserById(currentUser.Id);

          

            if (blResult.Errors.Count>0)
            {
                return View("ProfileLoadError",blResult.Errors);
            }
            return View(blResult.Result);
        }
        [Auth]
        [HttpGet]
        public ActionResult EditProfile()
        {
            BlogUser currentUser= CurrentSession.User;
           
            BussinesLayerResult<BlogUser> blResult = blogUserManager.GetUserById(currentUser.Id);
            if (blResult.Errors.Count>0)
            {
                return View("ProfileLoadError",blResult.Errors);
            }

            return View(blResult.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(BlogUser user, HttpPostedFileBase ProfileImage) 
        {
            // HttpPostedFileBase ile gönderilen dosyayı alabilmem için bu türde bir parametre tanımlamam/eklemem gerekiyor. Değişkenin ismi (ProfileImage), View tarafında input içerisinde name'e verdiğim deger ile aynı olmalı
            // Gönderilen dosyanın türün konstrol etmeliyim.. jpg,jpeg,png tüüründe olup olmadığını konstrol etmeliyim. Veritabanına hangi isim ile kaydedeceksem o ismi oluşturmalıyım ve daha sonra server tarafında Images klasörünün altında bu fotografı bu isimle kaydetmeliyim.
            // Dosya türünde kontrolünü ContentType ile yapıyorum.

            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (ProfileImage !=null && (
                    ProfileImage.ContentType =="image/jpg" ||
                    ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string fileName =$"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    // user_10.jpeg (.jpg -.png) gibi bir isim olusuyor
                    // Aşagıdaki kod ile birlikte fotografı ,server 'daki images klasörünün altına oluşturdugum dosya ismi ile kopyalıyorum.
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{fileName}"));
                    // Son olarak da dosya adının veritabanında tutulması gerekiyor
                    user.UserProfileImage=fileName;
                }

                // artık View'den gelen değişiklikleri veritabanına kaydetmek için gerekli kodları yazacagım.
                
                BussinesLayerResult<BlogUser> blResult = blogUserManager.UpdateProfile(user);
                if (blResult.Errors.Count>0)
                {
                    // hata oluştu demektir.
                    blResult.Errors.ForEach(x=>ModelState.AddModelError("",x));
                    return View(blResult.Result);
                }
                // hata yok ise
                //Session["login"] =blResult.Result;
                CurrentSession.Set<BlogUser>("login",blResult.Result);
                return RedirectToAction("ShowProfile");
            }
            return View(user);
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            BlogUser currentUser =CurrentSession.User;
            
            BussinesLayerResult<BlogUser> blResult = blogUserManager.DeleteUser(currentUser.Id);
            if (blResult.Errors.Count>0)
            {
                blResult.Errors.ForEach(x=>ModelState.AddModelError("",x));
                return View("ProfileLoadError" ,blResult.Errors);
            }
           // Session.Clear();
           CurrentSession.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
        

    }
}