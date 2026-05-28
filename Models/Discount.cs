using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public byte Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
