using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Toy
    {
        public Toy()
        {
            CartDetails = new HashSet<CartDetail>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
