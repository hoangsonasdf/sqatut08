using SQAtut08.Models;

namespace SQAtut08.Services
{
    public interface IUserService
    {
        string CreateToken(User user);
        User getCurrentUser();
    }
}
