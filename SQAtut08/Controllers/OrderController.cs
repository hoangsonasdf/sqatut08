using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQAtut08.DTO;
using SQAtut08.Models;
using SQAtut08.Services;

namespace SQAtut08.Controllers
{
    public class OrderController : Controller
    {
        private readonly SQAtut08Context _context;
        private readonly IUserService _userService;
        public OrderController(SQAtut08Context context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detail(DetailDTO request)
        {
            var size = await _context.Sizes
                .Where(s => s.Id == request.Size)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                })
                .SingleOrDefaultAsync();
            var topping = await _context.Toppings
                .Where(t => t.Id == request.Size)
                .Select(t => new
                {
                    t.Id, 
                    t.Name
                })
                .SingleOrDefaultAsync();
            ViewBag.size = size;
            ViewBag.topping = topping;
            ViewBag.user = _userService.getCurrentUser();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDTO request)
        {
            var newOrder = new Order
            {
                Sizeid = request.SizeID,
                Toppingid = request.ToppingID,
                Usersid = request.UserID
            };
            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("Successfully");
        }

        [HttpGet]
        public async Task<IActionResult> Successfully()
        {
            return View();
        }
    }
}
