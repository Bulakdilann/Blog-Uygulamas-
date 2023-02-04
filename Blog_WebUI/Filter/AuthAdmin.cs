using Blog_WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_WebUI.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        // standart/ normal/admin olmayan kullanıcılar için 
        // Category hepsi Access denied sayfasından yönlenmeli
        // BlogUser hepsi Access denied sayfasından yönlenmeli
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null && CurrentSession.User.IsAdmin==false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}