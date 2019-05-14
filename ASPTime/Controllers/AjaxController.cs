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
        public async Task<ActionResult> AddCategory(string categorynName)
        {
            if(categorynName == "" || categorynName == null)
            {
                return Json(new { Success = false, Message = "Ошибка добавления категории" });
            }
            var identityUserId = User.Identity.GetUserId();
            int categoryId = 0;
            using (var context = new ApplicationDbContext())
            {
                
                ApplicationUser user = new ApplicationUser();
                try
                {
                    user = context.Users.Where(o => o.Id == identityUserId).First();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найден" });
                }
                
                var alreadyExist = context.Categories.Where(o => o.Name == categorynName && o.User.Id == identityUserId).FirstOrDefault();
                if(alreadyExist == null)
                {
                    context.Categories.Add(new Category { Name = categorynName, User = user });
                    await context.SaveChangesAsync();
                    categoryId = context.Categories.Where(o => o.Name == categorynName && o.User.Id == identityUserId).Select(o => o.Id).FirstOrDefault();
                }
                else
                {
                    Debug.WriteLine("Ошибка добавления категории");
                    return Json(new { Success = false, Message = "Ошибка добавления категории" });
                }
            }
            return Json(new { Success = true, Name = categorynName, CategoryId = categoryId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(string categoryName)
        {
            if (categoryName == "" || categoryName == null)
            {
                return Json(new { Success = false, Message = "Ошибка Удаления категории" });
            }
            var identityUserId = User.Identity.GetUserId();
            int categoryId = 0;
            using (var context = new ApplicationDbContext())
            {
                ApplicationUser user = new ApplicationUser();
                try
                {
                    user = context.Users.Where(o => o.Id == identityUserId).First();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найден" });
                }
                var alreadyExist = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).FirstOrDefault();

                if(alreadyExist != null)
                {
                    categoryId = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).Select(o => o.Id).FirstOrDefault();
                    context.Categories.Remove(alreadyExist);
                    await context.SaveChangesAsync();
                }
                else
                {
                    Debug.WriteLine("Ошибка удаления категории");
                    return Json(new { Success = false, Message = "Ошибка ошибка удаления категории" });
                }
                return Json(new { Success = true, CategoryId = categoryId }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveTimeResult(string categoryName, int time)
        {
            if(categoryName == "" || categoryName == null || time == 0)
            {
                return Json(new { Success = false, Message = "Ошибка сохранения результата" });
            }
            var identityUserId = User.Identity.GetUserId();
            Category category = new Category();
            ApplicationUser user = new ApplicationUser();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    category = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).First();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Категория не найдена " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Категория не найдена" });
                }
                try
                {
                    user = context.Users.Where(o => o.Id == identityUserId).First();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найдены" });
                }
                try
                {
                    context.Times.Add(new TimeData
                    {
                        Date = DateTime.Now,
                        Time = time,
                        Category = category,
                        User = user
                    });
                    await context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Ошибка сохранения " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Ошибка сохранения" });
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveTimeResultFromInterval(string categoryName, int time, string dateIso)
        {
            if (categoryName == "" || categoryName == null || time == 0 || dateIso == "" || dateIso == null)
            {
                return Json(new { Success = false, Message = "Ошибка сохранения результата" });
            }
            var identityUserId = User.Identity.GetUserId();
            Category category = new Category();
            ApplicationUser user = new ApplicationUser();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    category = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).First();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Категория не найдена " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Категория не найдена" });
                }
                try
                {
                    user = context.Users.Where(o => o.Id == identityUserId).First();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найдены" });
                }
                try
                {
                    context.Times.Add(new TimeData
                    {
                        Date = DateTime.Parse(dateIso),
                        Time = time,
                        Category = category,
                        User = user
                    });
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка сохранения " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Ошибка сохранения" });
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}