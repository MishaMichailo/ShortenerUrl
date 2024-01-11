using ShortURL.Models;

namespace ShortURL.Services.Interfaces
{
    public interface IUserRepository
    {
        bool GetUserByName(string userName);
    }
}
