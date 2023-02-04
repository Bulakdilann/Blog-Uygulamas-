using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Filter
{
    public class HandleException : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // Eğer uygulamanın herhangi bir sayfasında bir hata oluşur da exception fırlatılırsa, sistemin göstereceği hata mesajı yerine benim tasarladıgım sayfa ve hata mesajı kullanıcıya gönderilecek.
            filterContext.Controller.TempData["LastError"]=filterContext.Exception;
            filterContext.ExceptionHandled=true;
            filterContext.Result=new RedirectResult("/Home/HasError");
        }
    }
}