using Kids_Toys.Data;
using Kids_Toys.Models;
using Kids_Toys.ModelViews;
using Microsoft.EntityFrameworkCore;

namespace Kids_Toys.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartDetail> GetCartItems()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return new List<CartDetail>();

            var cart = _context.carts.FirstOrDefault(c => c.UserId == userId && c.Status == 1);
            if(cart == null) return new List<CartDetail>();

            return _context.cart_Details.Where(cd => cd.CartId == cart.Id).Include(cd => cd.Toy).ToList();
        }

        public decimal GetCartTotalAmount()
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return 0;

            var cart = _context.carts.FirstOrDefault(c => c.UserId == userId && c.Status == 1);
            if (cart == null) return 0;
            return _context.cart_Details.Where(cd => cd.CartId == cart.Id).Sum(cd => (cd.Price - (decimal)cd.Discount) * (decimal)cd.Quentity);

        }

        public List<Category> GetParentCategories()
        {
            return _context.categories.Where(c => c.ParentId == null && c.StatusCat == true).OrderByDescending(t => t.CreatedAt).Take(4).ToList();
        }

    }
}
