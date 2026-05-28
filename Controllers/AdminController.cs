using Kids_Toys.Data;
using Kids_Toys.Models;
using Kids_Toys.ModelViews;
using Kids_Toys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kids_Toys.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(ILogger<AdminController> logger , ApplicationDbContext context , UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();

            List<UserRolesMV> result = new List<UserRolesMV>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserRolesMV { user = user, userRoles = (List<string>)roles });
            }

            ViewBag.allRoles = _roleManager.Roles.ToList();

            return View(result);
        }

        public async Task<IActionResult> addRoleToUser(string userid, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userid);

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }

            return RedirectToAction("Users");
        }

        public IActionResult ShowCategories()
        {
            var allCategories = _context.categories.ToList();
            return View(allCategories);
        }


        [HttpPost]
        public async Task<IActionResult> ManageCategories([FromForm] Category category, IFormFile? Image , string? ImagePath)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

                if (Image != null && Image.Length > 0)
                {
                    var extension = Path.GetExtension(Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest("The extension of the image is not allowed, the allowed extensions are: .jpg, .jpeg, .png, .webp");
                    }

                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(ImagePath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/categories/", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    category.Image = "/uploads/categories/" + fileName;
                }
                else
                {
                    category.Image = null;
                }

                if (category.Id == 0)
                {
                    category.CreatedAt = DateTime.Now;
                    category.UpdatedAt = DateTime.Now;
                    _context.categories.Add(category);
                    await _context.SaveChangesAsync();

                    string[] subCategories = {"Girls", "Boys"};
                    string[] subSubCategories = {"less than 2 years", "2 - 4 years", "5 - 7 years" , "8 - 10 years" , "11 - 14 years"};
                    foreach(var item in subCategories)
                    {
                       var subCategory = new Category
                        {
                            Name = @item,
                            StatusCat = true,
                            ParentId = category.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                        _context.categories.Add(subCategory);
                        await _context.SaveChangesAsync();

                        foreach (var subItem in subSubCategories)
                        {
                            var subSubCategory = new Category
                            {
                                Name = @subItem,
                                Description = "This category is for " + @item + " who are " + @subItem + " old.",
                                StatusCat = true,
                                ParentId = subCategory.Id,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                            };
                            _context.categories.Add(subSubCategory);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    category.UpdatedAt = DateTime.Now;
                    _context.categories.Update(category);
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Save category failed");
                return StatusCode(500, "Unexpected error");
            }

        }


        [HttpPost]
        public IActionResult DeleteCategory(int ID , string? ImagePath)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == ID);
            if (category == null)
                return NotFound();

            // حذف الصورة القديمة
            if (!string.IsNullOrEmpty(ImagePath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _context.categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }

        // Start Toys Controller
        public IActionResult ShowToys()
        {
            var allToys = _context.toys.Include(c => c.Category).ToList();
            return View(allToys);
        }

        [HttpPost]
        public async Task<IActionResult> ManageToys([FromForm] Toy model, IFormFile? Image ,  string? ImagePath)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            try
            {

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

            if (Image != null && Image.Length > 0)
            {
                var extension = Path.GetExtension(Image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("The extension of the image is not allowed, the allowed extensions are: .jpg, .jpeg, .png, .webp");
                }

                // Delete the old image
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/toys/", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                model.Image = "/uploads/toys/" + fileName;
            }
            else
            {
                model.Image = "NULL";
            }


                if (model.Id == 0)
            {

                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = DateTime.Now;
                _context.toys.Add(model);
            }
            else
            {
                model.UpdatedAt = DateTime.Now;
                _context.toys.Update(model);
            }

                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Save category failed");
                return StatusCode(500, "Unexpected error");
            }
        }

        [HttpPost]
        public IActionResult DeleteToy(int ID , string? ImagePath)
        {
            var toy = _context.toys.FirstOrDefault(c => c.Id == ID);
            if (toy == null)
                return NotFound();

            // Delete the old image
            if (!string.IsNullOrEmpty(ImagePath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _context.toys.Remove(toy);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.categories
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.ParentId
                })
                .ToList();

            return Json(categories);
        }

        public IActionResult ShowCarts()
        {
            var allCarts = _context.carts.ToList();
            return View(allCarts);
        }


        public IActionResult ManageCarts(Cart model)
        {
            if(model.Id != 0)
            {
                model.UpdatedAt = DateTime.Now;
                _context.carts.Update(model);
            }

            _context.SaveChanges();

            return Ok();
        }

        public IActionResult DeleteCart(int ID)
        {
            var cart = _context.carts.FirstOrDefault(c => c.Id == ID);
            if (cart == null)
                return NotFound();
            _context.carts.Remove(cart);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult ShowCartDetails()
        {
            var allCartDetails = _context.cart_Details.Include(cd => cd.Toy).ToList();
            return View(allCartDetails);
        }

        public IActionResult DeleteCartDetail(int ID)
        {
            var cartDetail = _context.cart_Details.FirstOrDefault(c => c.Id == ID);
            if (cartDetail == null)
                return NotFound();
            _context.cart_Details.Remove(cartDetail);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult ShowOrders()
        {
            var allOrders = _context.orders.ToList();
            return View(allOrders);
        }


        public IActionResult ManageOrders(Order model)
        {
            if (model.Id != 0)
            {
                var orderDetails = _context.order_Details.Where(cd => cd.OrderId == model.Id).ToList();
                model.UpdatedAt = DateTime.Now;
                _context.orders.Update(model);

                if(orderDetails != null)
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.Status = model.Status;
                    }
                }

            }

            _context.SaveChanges();

            return Ok();
        }

        public IActionResult DeleteOrder(int ID)
        {
            var order = _context.orders.FirstOrDefault(c => c.Id == ID);
            if (order == null)
                return NotFound();
            _context.orders.Remove(order);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult ShowOrderDetails()
        {
            var allOrderDetails = _context.order_Details.ToList();
            return View(allOrderDetails);
        }


        public IActionResult DeleteOrderDetail(int ID)
        {
            var orderDetail = _context.order_Details.FirstOrDefault(c => c.Id == ID);
            if (orderDetail == null)
                return NotFound();
            _context.order_Details.Remove(orderDetail);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult ShowComments()
        {
            var allComments = _context.comments.Include(c => c.Toy).ToList();
            return View(allComments);
        }
        public IActionResult ManageComments(Comment model)
        {
            if(model.Id != 0)
            {
                model.UpdatedAt = DateTime.Now;
                _context.comments.Update(model);
            }
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult DeleteComment(int ID)
        {
            var comment = _context.comments.FirstOrDefault(c => c.Id == ID);
            if (comment == null)
                return NotFound();
            _context.comments.Remove(comment);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult ShowDiscounts()
        {
            var allDiscounts = _context.discounts.ToList();
            return View(allDiscounts);
        }


        public IActionResult ManageDiscounts(Discount discount)
        {
            var minDate = new DateTime(1900, 1, 1);
            var maxDate = new DateTime(2077, 12, 31);

            if (discount.StartDate < minDate || discount.StartDate > maxDate ||
                discount.EndDate < minDate || discount.EndDate > maxDate)
            {
                return BadRequest("Invalid date range. Dates must be between 1900 and 2077.");
            }

            if (discount.Id == 0)
            {
                _context.discounts.Add(discount);
            }
            else
            {
                _context.discounts.Update(discount);
            }
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult DeleteDiscount(int ID)
        {
            var discount = _context.discounts.FirstOrDefault(c => c.Id == ID);
            if (discount == null)
                return NotFound();
            _context.discounts.Remove(discount);
            _context.SaveChanges();
            return Ok();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
