using System;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
using BCrypt.Net;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                    return null;

                // Verificar contraseña con BCrypt
                if (VerifyPassword(password, user.PasswordHash))
                {
                    return user;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            // Por ahora, el usuario con ID 1 es administrador
            // En producción, deberías tener una tabla de roles
            return userId == 1;
        }

        /// <summary>
        /// Genera un hash de contraseña usando BCrypt
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash de la contraseña</returns>
        public static string HashPassword(string password)
        {
            // Generar salt automáticamente y usar work factor 12 (balance entre seguridad y rendimiento)
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        /// <summary>
        /// Verifica si una contraseña coincide con su hash
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="hash">Hash de la contraseña</param>
        /// <returns>True si la contraseña coincide</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Si el hash no es válido de BCrypt, retornar false
                return false;
            }
        }

        /// <summary>
        /// Verifica si un hash es válido de BCrypt
        /// </summary>
        /// <param name="hash">Hash a verificar</param>
        /// <returns>True si el hash es válido</returns>
        public static bool IsValidHash(string hash)
        {
            try
            {
                // Intentar parsear el hash para verificar si es válido
                BCrypt.Net.BCrypt.InterrogateHash(hash);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
