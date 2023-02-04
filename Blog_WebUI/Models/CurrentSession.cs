using Blog_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog_WebUI.Models
{
    public class CurrentSession
    {
        public static BlogUser User
        {
            // bu metod static yapıyorum. newlemeden ulaşabilmek için .
            // Sesionda bulunan kullanıcının alınması için kullanılacak.
            get
            {
               //if( HttpContext.Current.Session["login"] != null)
               // {
               //     return HttpContext.Current.Session["login"] as BlogUser;
               // }
               return Get<BlogUser>("login");
            }
        }

        // Aşagıdaki metotta, Generic bir yapı kullandım. Sadece BlogUser türünde değil, faklı türden verileri de Sesionla'a koyabilmek için kullanacagım metottur.
        // CurrentSesion.Set<string>("Name","Dilan")
       // CurrentSesion.Set<BlogUser>("login",[BlogUser Türündeki nesneyi vereceğiz])
        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key]= obj;
        }

        // Session'daki veriyi almak için kullanacagım generic Get metodu.
        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] !=null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }

        // Sesionda bulunan bir verini parametresini vererek sessiondan kaldırmak ya da silmek için kullanacagım metot
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }

        }

        // Session'daki bütün veriyi temizliyor..
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();  
        }
}
}

// Sesion'da verinin saklanma süresi 20 dakikadır. Eğer sayfada her hangi bir işlem olmazsa ve 20dakika geçerse bu bilgi /veri sesiondan silinir.
// Sessionda tutulacak verinin tutulma süresini değiştirebiliriz.
// web.config dosyası içinde 
// Veri,timeout' a verdiğimiz sayısal değer kadar dakika sessionda tutulacak.
// <system.web>
// <sessionState mode="InProc" timeout="60">
//
// <system.web>