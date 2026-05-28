using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
