using Microsoft.AspNetCore.Mvc;
using ShortURL.Models;

namespace ShortURL.Services.Interfaces
{
    public interface IAuthService
    {
        public string CreateToken(User newUser);
        //public  Task<ActionResult<string>> RefreshToken(User newUser);
        //public AuthorizationResponse GenerateRefreshToken();
       // public void SetRefreshToken(AuthorizationResponse newRefreshToken, User newUser);
    }
}
