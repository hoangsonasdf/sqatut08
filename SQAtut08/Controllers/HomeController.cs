using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQAtut08.Models;
using SQAtut08.Services;
using System.Diagnostics;

namespace SQAtut08.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly SQAtut08Context _context;

        public HomeController(ILogger<HomeController> logger, IUserService userService, SQAtut08Context context)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public async Task<IActionResult> Hello()
        {
            var user = _userService.getCurrentUser();
            var topping = await _context.Toppings
                .Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
            var size = await _context.Sizes
                .Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
            ViewBag.user = user;
            ViewBag.topping = topping;
            ViewBag.size = size;
            return View();
        }
    }
}