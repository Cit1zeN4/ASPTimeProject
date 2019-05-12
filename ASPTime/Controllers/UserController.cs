using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASPTime.Models;
using Microsoft.AspNet.Identity;

namespace ASPTime.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public async Task<ActionResult> UserProfile(string userId)
        {
            string identityUserId = User.Identity.GetUserId();
            if (identityUserId == userId)
            {
                var viewModel = new UserProfileViewModel();
                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        viewModel.Categories = await context.Categories.Where(o => o.User.Id == identityUserId).ToListAsync();                        
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Ошибка поиска категорий у пользователя: " + ex.Source + " " + ex.Message);
                    }
                    
                }
                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
};