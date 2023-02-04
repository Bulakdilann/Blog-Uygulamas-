using Blog_BussinesLayer;
using Blog_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Blog_WebUI.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            // veriyi memory'e set ediyoruz.
            // memory'den alıyoruz
            // sonuçları bize hızlı bir şekilde getiriyor.
            // ilk drowdownı veritabanından çeker sonra cache gelir. cachede olunca veri ekleyince direk gelmiyor ama bunu categorymanagerda duzenledik. 
            // WebHlper içinde bulunan WebCach isimli Class'ı kullanacagız.
            var result =WebCache.Get("category");
            if (result==null)
            {
                CategoryManager categoryManager=new CategoryManager();
                result=categoryManager.List();
                //  WebCache.Set("key",value,ne kadar süre cachede kalaagına dair olan değer(dakika cinsi), [Her kullanımda Cache'te kalma süresi verilen değer kadar ötelenecek]);
                WebCache.Set("category",result,20,true);
            }
            return result;
        }
        public static void RemoveCategoriesFromCache() //string yazacan
        {
            /// burayı yapmaya çalıs generic hale getir. currentsessiona bak
             WebCache.Remove("category");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}