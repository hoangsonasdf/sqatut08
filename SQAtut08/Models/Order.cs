using System;
using System.Collections.Generic;

namespace SQAtut08.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? Usersid { get; set; }
        public int? Sizeid { get; set; }
        public int? Toppingid { get; set; }

        public virtual Size? Size { get; set; }
        public virtual Topping? Topping { get; set; }
        public virtual User? Users { get; set; }
    }
}
