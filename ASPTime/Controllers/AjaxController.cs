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

        [HttpPost]
        public ActionResult AddCategory(string name)
        {
            if(name == "" || name == null)
            {
                return Json(new { Success = false, Message = "Ошибка добавления категории" });
            }
            var identityUserId = User.Identity.GetUserId();
            int categoryId = 0;
            using (var context = new ApplicationDbContext())
            {
                var userid = User.Identity.GetUserId();
                ApplicationUser user = new ApplicationUser();
                try
                {
                    user = context.Users.Where(o => o.Id == userid).First();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найден" });
                }
                
                var alreadyExist = context.Categories.Where(o => o.Name == name && o.User.Id == identityUserId).FirstOrDefault();
                if(alreadyExist == null)
                {
                    context.Categories.Add(new Category { Name = name, User = user });
                    context.SaveChanges();
                    categoryId = context.Categories.Where(o => o.Name == name & o.User.Id == userid).Select(o => o.Id).FirstOrDefault();
                }
                else
                {
                    Debug.WriteLine("Ошибка добавления категории");
                    return Json(new { Success = false, Message = "Ошибка добавления категории" });
                }
            }
            return Json(new { Success = true, Name = name, CategoryId = categoryId }, JsonRequestBehavior.AllowGet);
        }
    }
}