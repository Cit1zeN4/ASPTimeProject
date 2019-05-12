namespace ASPTime.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}