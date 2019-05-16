namespace ASPTime.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual List<TimeData> Times { get; set; }
    }

    public class TimeData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Time { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}