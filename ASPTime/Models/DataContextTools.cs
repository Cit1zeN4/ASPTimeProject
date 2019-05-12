using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace ASPTime.Models
{
    public class DataContextTools
    {
        public static void SetDefaultCategories(string userId)
        {
            string message = "";
            using (var context = new ApplicationDbContext())
            {
                ApplicationUser user = new ApplicationUser();
                try
                {
                    user = context.Users.Where(o => o.Id == userId).First();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка поиска пользователя: " + ex.Source + " " + ex.Message);
                }

                IEnumerable<Category> defaultCategories = new List<Category>
                {
                    new Category {Name = "Спорт", User = user},
                    new Category {Name = "Работа", User = user},
                    new Category {Name = "Учеба", User = user}
                };

                context.Categories.AddRange(defaultCategories);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        message = "Object: " + validationError.Entry.Entity.ToString();

                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            message = message + err.ErrorMessage + "";
                        }
                    }
                    Debug.WriteLine(message);
                }
            }
        }
    }
}