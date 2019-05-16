using ASPTime.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

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
        public async Task<ActionResult> AddCategory(string categoryName)
        {
            if(categoryName == "" || categoryName == null)
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
                
                var alreadyExist = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).FirstOrDefault();
                if(alreadyExist == null)
                {
                    context.Categories.Add(new Category { Name = categoryName, User = user });
                    await context.SaveChangesAsync();
                    categoryId = context.Categories.Where(o => o.Name == categoryName && o.User.Id == identityUserId).Select(o => o.Id).FirstOrDefault();
                }
                else
                {
                    Debug.WriteLine("Ошибка добавления категории");
                    return Json(new { Success = false, Message = "Ошибка добавления категории" });
                }
            }
            return Json(new { Success = true, Name = categoryName, CategoryId = categoryId }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetStatisticsForToday()
        {
            var identityUserId = User.Identity.GetUserId();
            List<Category> categories = new List<Category>();
            Chart chart = new Chart();
            Chart chart1 = new Chart();
            Chart chart2 = new Chart();
            Chart chart3 = new Chart();
            List<ChartPieValue> chartPieValues = new List<ChartPieValue>();
            ChartDataAdapter adapter = new ChartDataAdapter();
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    categories = context.Users.Where(o => o.Id == identityUserId)
                        .Select(o => o.UserCategories).First();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Пользователь не найден " + ex.Source + " " + ex.Message);
                    return Json(new { Success = false, Message = "Пользователь не найден" });
                }
                DateTime toDay = DateTime.Now;
                int i = 0;
                foreach (var item in categories)
                {
                    chartPieValues.Add(new ChartPieValue
                    {
                        Label = item.Name,
                        Value = Math.Round(item.Times.Where(o => o.Date.Year == toDay.Year && o.Date.Month == toDay.Month && o.Date.Day == toDay.Day)
                        .Select(o => o.Time).Sum() / 60 / 60, 2),
                        BackgroundColor = adapter.BackgroundColor[i]
                    });
                    i++;
                    if (i == 4)
                        i = 0;
                }
                chart = adapter.GetChart(chartPieValues);
                chartPieValues = new List<ChartPieValue>();
                i = 0;
                foreach (var item in categories)
                {
                    chartPieValues.Add(new ChartPieValue
                    {
                        Label = item.Name,
                        Value = Math.Round(item.Times.Where(o => o.Date.Year == toDay.Year && o.Date.Month == toDay.Month)
                        .Select(o => o.Time).Sum() / 60 / 60, 2),
                        BackgroundColor = adapter.BackgroundColor[i]
                    });
                    i++;
                    if (i == 4)
                        i = 0;
                }
                chart1 = adapter.GetChart(chartPieValues);
                chartPieValues = new List<ChartPieValue>();
                i = 0;
                foreach (var item in categories)
                {
                    chartPieValues.Add(new ChartPieValue
                    {
                        Label = item.Name,
                        Value = Math.Round(item.Times.Where(o => o.Date.Year == toDay.Year)
                        .Select(o => o.Time).Sum() / 60 / 60, 2),
                        BackgroundColor = adapter.BackgroundColor[i]
                    });
                    i++;
                    if (i == 4)
                        i = 0;
                }
                chart2 = adapter.GetChart(chartPieValues);
                chartPieValues = new List<ChartPieValue>();
                i = 0;
                foreach (var item in categories)
                {
                    chartPieValues.Add(new ChartPieValue
                    {
                        Label = item.Name,
                        Value = Math.Round(item.Times.Select(o => o.Time).Sum() / 60 / 60, 2),
                        BackgroundColor = adapter.BackgroundColor[i]
                    });
                    i++;
                    if (i == 4)
                        i = 0;
                }
                chart3 = adapter.GetChart(chartPieValues);
                var jsonstr = JsonConvert.SerializeObject(new { Success = true, Value = new { Val1 = chart, Val2 = chart1, Val3 = chart2, Val4 = chart3 } });
                JsonResult json = new JsonResult
                {
                    Data = JsonConvert.DeserializeObject(jsonstr)
                };

                return Content(jsonstr, "application/json");
            }
        }
    }
}