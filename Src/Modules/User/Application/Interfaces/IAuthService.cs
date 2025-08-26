using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserEntity?> LoginAsync(string email, string password);
        Task<bool> IsAdminAsync(int userId);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        bool IsValidHash(string hash);
    }
}
