using System.ComponentModel.DataAnnotations;

namespace SQAtut08.DTO
{
    public class RegistorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
