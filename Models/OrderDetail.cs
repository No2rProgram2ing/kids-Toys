using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int ToyId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int? Discount { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
