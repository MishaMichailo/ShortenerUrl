using Microsoft.AspNetCore.Mvc;
using System.Text;
using ShortURL.Data;
using ShortURL.Models;
using ShortURL.Services.Interfaces;
using ShortURL.DTO;
using System.Security.Cryptography;

namespace ShortURL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly UrlShortenerContext _context;
        private readonly IUserRepository _userRepository;
        public RegistrationController(UrlShortenerContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] RegistrationDTO newUser,string? role)
        {
            if (_userRepository.GetUserByName(newUser.Name))
            {
                return BadRequest("User with this username already exists.");
            }
            CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            if (ModelState.IsValid)
            {
                User user = new()
                {
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = newUser.Email,
                    Name = newUser.Name,
                    Password = newUser.Password,
                    Role = role
                };
                RegLog regLog = new()
                {
                    RegisterTime = DateTime.UtcNow,
                };
                user.RegLogs = regLog;
                _context.Users.Add(user);
                _context.SaveChanges();

                int userId = user.Id;
                return Ok(new { Id = userId, Message = "User registered successfully." });

            }
            return BadRequest("User registration failed");
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
