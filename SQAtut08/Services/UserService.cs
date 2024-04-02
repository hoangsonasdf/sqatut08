using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SQAtut08.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SQAtut08.Services
{
    public class UserService : IUserService
    {
        private readonly SQAtut08Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(SQAtut08Context context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string CreateToken(User user)
        {
  
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public User getCurrentUser()
        {
            // Retrieve the JWT token from the cookie
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            if (string.IsNullOrEmpty(token))
            {
                return null; // Token is missing or invalid
            }

            // Validate and decode the JWT token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null; // Token validation failed
            }

            // Extract user information from claims
            string email = principal.FindFirst(ClaimTypes.Name)?.Value;
            // Retrieve additional user details as needed

            // Create and return a User object
            return _context.Users
                .Where(u => u.Email == email)
                .SingleOrDefault();
        }

    }
}
