using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task AddUserAsync(UserEntity user)
        {
            // Aquí podrías agregar validaciones, hash de password, etc.
            await _repository.AddAsync(user);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}