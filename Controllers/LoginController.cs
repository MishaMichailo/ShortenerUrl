using Microsoft.AspNetCore.Mvc;
using System.Text;
using ShortURL.Data;
using ShortURL.Models;
using ShortURL.Services.Interfaces;
using ShortURL.DTO;
using System.Security.Cryptography;
using System.Data;

namespace ShortURL.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class LoginController : Controller
    {
        private readonly UrlShortenerContext _context;
        private readonly IAuthService _authService;

        public LoginController(UrlShortenerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public  IActionResult Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var userFromDb = _context.Users.FirstOrDefault(u => u.Name == loginDto.Name);
                if (userFromDb == null)
                {
                    return Unauthorized();
                }

                if (!VerifyPasswordHash(userFromDb.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }

                var log = new LoginLog()
                {
                    LoginTime = DateTime.Now,
                    UserId = userFromDb.Id,
                };

                _context.LoginLogs.Add(log);
                _context.SaveChanges();

                var isAdmin = userFromDb.Role == "admin";
                var token = _authService.CreateToken(userFromDb);
                var userId = userFromDb.Id;

                return Ok(new { userId,token, isAdmin });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
