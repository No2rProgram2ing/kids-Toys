using Kids_Toys.Data;
using Kids_Toys.Models;
using Kids_Toys.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security;
namespace Kids_Toys.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult GetStarted()
        {
            return View();
        }

        public IActionResult Index()
        {
            //var allCategories = _context.categories.Where(c => c.ParentId != null).ToList();
            var mainCategories = _context.categories.Where(c => c.StatusCat == true && c.ParentId == null).OrderByDescending(c => c.CreatedAt).Take(4).ToList();
            var allDiscounts = _context.toys.Where(t => t.Discount != 0).OrderByDescending(t => t.CreatedAt).Take(4).ToList();
            var allNewToys = _context.toys.OrderByDescending(t => t.CreatedAt).Take(4).ToList();
            var girlsCategory = _context.categories.Where(gc => gc.Name == "Girls").Select(gc => gc.Id).ToList();
            var boysCategory = _context.categories.Where(bc => bc.Name == "Boys").Select(bc => bc.Id).ToList();
            var subGirlsCategory = _context.categories.Where(sgc => sgc.ParentId != null && girlsCategory.Contains(sgc.ParentId.Value)).Select(sgc => sgc.Id).ToList();
            var subBoysCategory = _context.categories.Where(sbc => sbc.ParentId != null && boysCategory.Contains(sbc.ParentId.Value)).Select(sgc => sgc.Id).ToList();
            var girlsToies = _context.toys.Where(t => subGirlsCategory.Contains(t.CategoryId)).OrderByDescending(t => t.CreatedAt).Take(4).ToList();
            var boysToies = _context.toys.Where(t => subBoysCategory.Contains(t.CategoryId)).OrderByDescending(t => t.CreatedAt).Take(4).ToList();
            var comments = _context.comments.OrderByDescending(c => c.CreatedAt).Take(4).ToList();


            var CategoryToy = new CategoryToyMV
            {
                //Categories = allCategories,
                MainCategories = mainCategories,
                AllDiscounts = allDiscounts,
                AllNew = allNewToys,
                GirlsToies = girlsToies,
                BoysToies = boysToies,
                Comments = comments,
            };

            return View(CategoryToy);
        }

        public IActionResult Category()
        {
            var mainCategories = _context.categories.Where(c => c.StatusCat == true && c.ParentId == null).ToList();
            var categories = new CategoryToyMV
            {
                MainCategories = mainCategories,
            };
            return View(categories);
        }

        public IActionResult SubCategory(string name)
        {

            var subCategory = _context.categories.FirstOrDefault(c => c.Name == name);
            var subSubCategories = _context.categories.Where(c => c.ParentId == subCategory.Id).ToList();

            var subCategoriesMV = new CategoryToyMV
            {
                SubSubCategories = subSubCategories,
                CategoryName = name,
            };

            return View(subCategoriesMV);
        }


        public IActionResult DefineAges(string name , string subName)
        {
            var subCategory = _context.categories.Where(c => c.Name == name).Select(c => c.Id).ToList();
            var subSubCategory = _context.categories.Where(c => subCategory.Contains(c.ParentId.Value)  &&  c.Name == subName).Select(c => c.Id).ToList();
            var allToys = _context.toys.Where(t => subSubCategory.Contains(t.CategoryId)).ToList();

            var ToysBasedOnAge = new CategoryToyMV
            {
                Toies = allToys,
                CategoryName = subName,
                SubCategoryName = name,
            };
            return View(ToysBasedOnAge);
        }

        public IActionResult ToyDetail(int id)
        {
            var toyDetail = _context.toys.FirstOrDefault(t => t.Id == id);
            var comments = _context.comments.Where(c => c.ToyId == id).ToList();
            var toyDetailMV = new CategoryToyMV
            {
                ToyDetail = toyDetail,
                Comments = comments,
            };

            return View(toyDetailMV);
        }

        public IActionResult CategoryToies(int ID)
        {
            var mainCategory = _context.categories.FirstOrDefault(c => c.ParentId == null && c.Id == ID);
            var subCategoies = _context.categories.Where(c => c.ParentId == ID).Select(c => c.Id).ToList();
            var subSubCategories = _context.categories.Where(c => subCategoies.Contains(c.ParentId.Value)).Select(c => c.Id).ToList();
            var categoryToies = _context.toys.Where(t => subSubCategories.Contains(t.CategoryId)).ToList();

            var categoryToiesMV = new CategoryToyMV
            {
                CategoryName = mainCategory.Name,
                CategoryDescription = mainCategory.Description,
                Toies = categoryToies,
            };

            return View(categoryToiesMV);
        }

        public IActionResult NewToies()
        {
            var allNewToys = _context.toys.OrderByDescending(t => t.CreatedAt).Take(15).ToList();

            var newToies = new CategoryToyMV
            {
                AllNew = allNewToys,
            };

            return View(newToies);
        }



        public IActionResult Discounts()
        {
            var allDiscounts = _context.toys.Where(t => t.Discount != 0).OrderByDescending(t => t.CreatedAt).ToList();

            var discounts = new CategoryToyMV
            {
                AllDiscounts = allDiscounts,
            };

            return View(discounts);
        }

        public IActionResult AddToCart(int id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var cart = _context.carts.FirstOrDefault(c => c.UserId == userId && c.Status == 1);
            if (cart == null)
            {
                cart = new Cart     
                {
                    UserId = userId,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.carts.Add(cart);
                _context.SaveChanges();
            }

            var existingCartItem = _context.cart_Details.FirstOrDefault(item => item.ToyId == id && item.CartId == cart.Id);

            if (existingCartItem != null)
            {
                existingCartItem.Quentity++;
                existingCartItem.UpdatedAt = DateTime.Now;
            }
            else
            {
                var toySelected = _context.toys.FirstOrDefault(t => t.Id == id);
                if (toySelected == null) return NotFound();

                _context.cart_Details.Add(new CartDetail
                {
                    ToyId = toySelected.Id,
                    CartId = cart.Id,
                    Price = toySelected.Price,
                    Discount = Convert.ToInt32(toySelected.Discount),
                    Quentity = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int ID)
        {
            var cartDetail = _context.cart_Details.FirstOrDefault(cd => cd.Id == ID);
            _context.cart_Details.Remove(cartDetail);
            _context.SaveChanges();

            return Ok();
        }
        public IActionResult AddOrder(Order model)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var cart = _context.carts.FirstOrDefault(c => c.UserId == userId && c.Status == 1);

            var cartDetails = _context.cart_Details.Where(cd => cd.CartId == cart.Id).ToList();

            var discount = _context.discounts.FirstOrDefault(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now && d.Status == 1);

            if (userId == null)
                return NotFound();
            if (cart != null)
            {
                cart.Status = 2;

                if (model.Id == 0)
                {
                    if (discount != null)
                    {
                        model.DiscountId = discount.Id;
                        model.TotalAmount -= discount.DiscountPercentage;
                    }
                    model.UserId = userId;
                    model.CreatedAt = DateTime.Now;
                    model.UpdatedAt = DateTime.Now;
                    _context.orders.Add(model);
                    _context.SaveChanges();

                    foreach (var cartDetail in cartDetails)
                    {
                        _context.order_Details.Add(new OrderDetail
                        {
                            Price = cartDetail.Price,
                            OrderId = model.Id,
                            Status = model.Status,
                            TotalPrice = (cartDetail.Price  - Convert.ToDecimal(cartDetail.Discount)) * Convert.ToDecimal(cartDetail.Quentity),
                            Discount = cartDetail.Discount,
                            ToyId = cartDetail.ToyId,
                            Quantity = cartDetail.Quentity,
                            CreatedAt = cartDetail.CreatedAt,
                            UpdatedAt = cartDetail.UpdatedAt,
                        });
                    }

                }

            }
            else
            {
                return NotFound();
            }
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult AddComment(Comment model)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            if (model.Id == 0)
            {
                model.UserId = userId;
                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = DateTime.Now;
                _context.comments.Add(model);
            }
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult IncreaseQuantity(int ID)
        {
            var cartItem = _context.cart_Details.FirstOrDefault(cd => cd.Id == ID);
            if (cartItem == null) return NotFound();
            cartItem.Quentity++;
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult DecreaseQuantity(int ID)
        {
            var cartItem = _context.cart_Details.FirstOrDefault(cd => cd.Id == ID);
            if (cartItem == null) return NotFound();
            if(cartItem.Quentity == 1)
            {
                cartItem.Quentity = 0;
                _context.cart_Details.Remove(cartItem);
            }
            else
            {
                cartItem.Quentity--;
            }
            _context.SaveChanges();
            return Ok();
        }


        public IActionResult AboutUs()
        {
            return View();
        }


        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
