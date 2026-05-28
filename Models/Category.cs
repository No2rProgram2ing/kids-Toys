using System;
using System.Collections.Generic;

namespace Kids_Toys.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseParent = new HashSet<Category>();
            Toys = new HashSet<Toy>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool? StatusCat { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category> InverseParent { get; set; }
        public virtual ICollection<Toy> Toys { get; set; }
    }
}
