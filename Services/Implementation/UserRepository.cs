using ShortURL.Data;
using ShortURL.Models;
using ShortURL.Services.Interfaces;

namespace ShortURL.Services.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UrlShortenerContext _context;
        public UserRepository(UrlShortenerContext context)
        {
            _context = context;
        }

        public bool GetUserByName(string userName)
        {
            return _context.Users.Any(u => u.Name == userName);
        }
    }
}
