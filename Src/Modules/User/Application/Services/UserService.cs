using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            // Validar que el email no esté duplicado
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            // Validar edad mínima
            var age = DateTime.Today.Year - user.Birthdate.Year;
            if (user.Birthdate.Date > DateTime.Today.AddYears(-age)) age--;
            
            if (age < 18)
            {
                throw new InvalidOperationException("El usuario debe ser mayor de 18 años");
            }

            // Hashear la contraseña con BCrypt antes de guardar
            user.PasswordHash = AuthService.HashPassword(user.PasswordHash);
            
            // Establecer timestamps
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.UserId);
            if (existingUser == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            // Validar edad mínima si se cambió la fecha de nacimiento
            if (user.Birthdate != existingUser.Birthdate)
            {
                var age = DateTime.Today.Year - user.Birthdate.Year;
                if (user.Birthdate.Date > DateTime.Today.AddYears(-age)) age--;
                
                if (age < 18)
                {
                    throw new InvalidOperationException("El usuario debe ser mayor de 18 años");
                }
            }

            // Si se cambió la contraseña, hashearla con BCrypt
            if (user.PasswordHash != existingUser.PasswordHash && 
                !AuthService.IsValidHash(user.PasswordHash))
            {
                user.PasswordHash = AuthService.HashPassword(user.PasswordHash);
            }

            // Actualizar timestamp
            user.UpdatedAt = DateTime.Now;

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Verificar contraseña actual
            if (!AuthService.VerifyPassword(currentPassword, user.PasswordHash))
            {
                return false;
            }

            // Validar nueva contraseña (puedes agregar más validaciones aquí)
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                throw new InvalidOperationException("La nueva contraseña debe tener al menos 6 caracteres");
            }

            // Hashear nueva contraseña
            user.PasswordHash = AuthService.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> ValidatePasswordAsync(int userId, string password)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return AuthService.VerifyPassword(password, user.PasswordHash);
        }
    }
}