using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using SQAtut08.DTO;
using SQAtut08.Models;
using SQAtut08.Services;

namespace SQAtut08.Controllers
{
    public class AuthController : Controller
    {
        private readonly SQAtut08Context _context;
        private readonly IUserService _userService;

        public AuthController(SQAtut08Context context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistorDTO request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                ViewBag.Error = new { errorMessage = "*Password does not match" };
                return View();
            }
            var checkuser = await _context.Users
                            .Where(x => (x.Email == request.Email))
                            .SingleOrDefaultAsync();
            if (checkuser == null)
            {
                var new_user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    Role = "user"
                };
                await _context.AddAsync(new_user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            ViewBag.Error = new { errorMessage = "*Email has already existed" };
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await _context.Users
                .Where(u => u.Email == request.Email && u.Password == request.Password)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                ViewBag.Error = new { errorMessage = "*Email or Password invalid" };
                return View();
            }

            var token = _userService.CreateToken(user);
            Response.Cookies.Append("token", token);
            return RedirectToAction("Hello", "Home");
        }
    }
}
