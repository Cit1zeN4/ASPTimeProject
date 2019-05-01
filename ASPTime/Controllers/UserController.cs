using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ASPTime.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public ActionResult UserProfile(string userId)
        {
            if (User.Identity.GetUserId() == userId)
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
};