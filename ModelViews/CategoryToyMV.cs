using Kids_Toys.Models;

namespace Kids_Toys.ModelViews
{
    public class CategoryToyMV
    {
        public List<Category> Categories { set; get; }

        public List<Category> MainCategories { set; get; }

        public List<Category> SubCategories { set; get; }

        public List<Category> SubSubCategories { set; get; }

        public List<Toy> Toies { set; get; }

        public List<Toy> GirlsToies { set; get; }

        public List<Toy> BoysToies { set; get; }

        public List<Toy> AllNew { set; get; }

        public List<Toy> AllDiscounts { set; get; }

        public Toy ToyDetail { set; get; }

        public string CategoryName { set; get; }
        public string SubCategoryName { set; get; }

        public string CategoryDescription { set; get; }

        public List<Comment> Comments { set; get; }
    }
}
