using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string ToyComment { get; set; } = null!;
        public int ToyId { get; set; }
        public string UserId { get; set; } = null!;
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Toy Toy { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
