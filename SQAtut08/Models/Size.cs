using System;
using System.Collections.Generic;

namespace SQAtut08.Models
{
    public partial class Size
    {
        public Size()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
