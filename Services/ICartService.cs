using Kids_Toys.Models;

namespace Kids_Toys.Services
{
    public interface ICartService
    {
        List<CartDetail> GetCartItems();

        decimal GetCartTotalAmount();

        List<Category> GetParentCategories();

    }

}
