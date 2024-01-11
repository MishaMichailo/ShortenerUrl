using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShortURL.Models;
using ShortURL.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShortURL.Services.Implementation
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _config;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _config = configuration;
            //_httpContextAccessor = httpContextAccessor;
        }
        public string CreateToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
            };
            if (!string.IsNullOrEmpty(user.Role) && user.Role.ToLower() == "admin")
            {
                userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtSettings:Key"]!));
            

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("JwtSettings:Issuer").Value,
                audience: _config.GetSection("JwtSettings:Audience").Value,
                claims: userClaims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(3)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public async Task<ActionResult<string>> RefreshToken(User newUser)
        //{
        //    var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

        //    if (!newUser.RefreshToken.Equals(refreshToken))
        //    {
        //        return new ObjectResult("Invalid Refresh Token.")
        //        {
        //            StatusCode = StatusCodes.Status401Unauthorized
        //        };
        //    }
        //    else if (newUser.TokenExpires < DateTime.Now)
        //    {
        //        return new ObjectResult("Token expired.")
        //        {
        //            StatusCode = StatusCodes.Status401Unauthorized
        //        };
        //    }

        //    AuthorizationResponse token = CreateToken(newUser);
        //    var newRefreshToken = GenerateRefreshToken();
        //    SetRefreshToken(newRefreshToken, newUser);

        //    return new ObjectResult(token);
        //}

        //public AuthorizationResponse GenerateRefreshToken()
        //{
        //    var refreshToken = new AuthorizationResponse
        //    {
        //        AuthorizationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        Expires = DateTime.Now.AddDays(7),
        //        Created = DateTime.Now
        //    };

        //    return refreshToken;
        //}

        //public void SetRefreshToken(AuthorizationResponse newRefreshToken, User newUser)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires = newRefreshToken.Expires
        //    };
        //    _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.AuthorizationToken, cookieOptions);


        //    newUser.RefreshToken = newRefreshToken.AuthorizationToken;
        //    newUser.TokenCreated = newRefreshToken.Created;
        //    newUser.TokenExpires = newRefreshToken.Expires;


        //}

    }
}


