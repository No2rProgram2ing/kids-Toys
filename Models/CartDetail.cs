using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class CartDetail
    {
        public int Id { get; set; }
        public int ToyId { get; set; }
        public int CartId { get; set; }
        public decimal Price { get; set; }
        public int Quentity { get; set; }
        public int? Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual Toy Toy { get; set; } = null!;
    }
}
