using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortURL.Data;
using ShortURL.DTO;
using ShortURL.Models;
using ShortURL.Services.Implementation;
using System.Security.Claims;

namespace ShortURL.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShortenerUrlController : Controller
    {
        private readonly UrlShortenerService _urlShortener;
        private readonly UrlShortenerContext _context;
        public ShortenerUrlController(UrlShortenerService urlShortener, UrlShortenerContext context)
        {
            _urlShortener = urlShortener;
            _context = context;
        }

        [HttpPost("shorturl")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ShortneURL([FromBody] UrlLogDTO logUrl)
        {
            var userFromDb = _context.Users.FirstOrDefault(u => u.Id == logUrl.UserId);
            var receivedToken = HttpContext.Request.Headers["Authorization"];
            if (userFromDb == null)
            {
                return Unauthorized();
            }
            if (logUrl.UrlBase == null)
            {
                return BadRequest("Please provide a valid URL ");
            }
            string shortCode = _urlShortener.GenerateUniqueCode();
            string shortUrl = GetBaseUrlFromApiUser() + shortCode;

            var urlLog = new UrlLog
            {
                UrlBase = logUrl.UrlBase,
                UrlShort = shortUrl,
                UserId = logUrl.UserId,
            };
            var userId = logUrl.UserId;
            _context.UrlLogs.Add(urlLog);
            _context.SaveChanges();

            return Ok(new { userId, ShortUrl = shortUrl });
        }
        [HttpGet("user/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserUrls(int userId)
        {
            try
            {
                var userFromDb = _context.Users.Any(u => u.Id == userId);
                var receivedToken = HttpContext.Request.Headers["Authorization"];
                if (!userFromDb)
                {
                    return Unauthorized();
                }

                var rows = _context.UrlLogs
                    .Where(u => u.UserId == userId)
                    .GroupBy(u => u.UrlBase)
                    .Select(g => g.First())
                    .ToList();

                return Ok(new { userId, rows });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("~/{shortCode}", Name = "ShortCode")]
        public IActionResult RedirectShortURL(string shortCode)
        {
            var urlLog = _context.UrlLogs.FirstOrDefault(log => log.UrlShort == GetBaseUrlFromApiUser() + shortCode);
            if (urlLog != null)
            {
                return Redirect(urlLog.UrlBase);
            }
            return NotFound();
        }
        [HttpPost("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteURL([FromBody] UrlLogDTO deleteRequest)
        {
            var userFromDb = _context.Users.Any(u => u.Id == deleteRequest.UserId);
            var receivedToken = HttpContext.Request.Headers["Authorization"];
            if (!userFromDb)
            {
                return Unauthorized();
            }

            var urlLogs = _context.UrlLogs.Where(log => log.UserId == deleteRequest.UserId && log.UrlBase == deleteRequest.UrlBase).ToList();
            if (urlLogs.Any())
            {
                foreach (var urlLog in urlLogs)
                {
                    _context.UrlLogs.Remove(urlLog);
                }
                _context.SaveChanges();

                return Ok(new { Message = "URLs deleted successfully." });
            }

            return NotFound();
        }
        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAllUsersInfo()
        {
            var usersFromDb = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Role,
                    LoginsCount = _context.LoginLogs.Where(user => user.UserId == u.Id).Count()
                })
                .ToList();

            return Ok(usersFromDb);
        }
        private static string GetBaseUrlFromApiUser()
        {
            return "http://urlshort1-dev.eba-svf38q8a.us-east-1.elasticbeanstalk.com/";
        } 
    }
}
