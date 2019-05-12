using ASPTime.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASPTime.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Timer");
            else
                return RedirectToAction("About");     
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Timer()
        {
            var viewModel = new TimerViewModel();
            var identityUserId = User.Identity.GetUserId();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    viewModel.Categories = await context.Categories.Where(o => o.User.Id == identityUserId).ToListAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка поиска категорий у пользователя: " + ex.Source + " " + ex.Message);
                }

            }
            return View(viewModel);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}