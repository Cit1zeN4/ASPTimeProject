using ASPTime.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASPTime.Controllers
{
    public class AjaxController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> ChangeCategoryName(string oldName, string newName)
        {
            var identityUserId = User.Identity.GetUserId();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var category = context.Categories.Where(o => o.Name == oldName && o.User.Id == identityUserId).First();
                    category.Name = newName;
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка изменения названия категории " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false });
                }
            }
            return Json(new { Success = true, Name = newName }, JsonRequestBehavior.AllowGet);
        }
    }
}