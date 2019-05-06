using Girafile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Girafile.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return View("Landing");  
            }

            
        }
    }
}
