using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> AddUserAsync(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(int id);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> UpdateUserAsync(UserEntity user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ValidatePasswordAsync(int userId, string password);
    }
}
