using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; } = null!;
        public int? DiscountId { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public byte PaymentMethod { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Discount? Discount { get; set; }
        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
