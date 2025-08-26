using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.Users.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Users.Domain.Entities;
using campuslovejorge_edwin.Src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Modules.Users.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            // 1. Buscar o crear la profesión
            var profesion = await _context.Professions
                .FirstOrDefaultAsync(p => p.Description == dto.Profesion);

            if (profesion == null && !string.IsNullOrWhiteSpace(dto.Profesion))
            {
                profesion = new Profession { Description = dto.Profesion };
                _context.Professions.Add(profesion);
                await _context.SaveChangesAsync();
            }

            // 2. Crear el perfil
            var profile = new Profile
            {
                Name = dto.Nombre ?? "",
                Lastname = dto.Apellido ?? "",
                Identification = dto.Identificacion ?? "",
                Gender_Id = dto.Gender_Id,
                Slogan = dto.FrasePerfil ?? "",
                Status_Id = 1,
                Profession_Id = profesion.Id, // usamos el ID de la profesión creada o encontrada
                CreateDate = DateTime.Now,
                Total_Likes = 0
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            // 3. Crear usuario
            var user = new Usuario
            {
                Username = dto.Username ?? "",
                Password = dto.Password ?? "",
                BirthDate = dto.FechaNacimiento,
                Profile_Id = profile.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 4. Procesar intereses (buscar o crear)
            foreach (var interesNombre in dto.Intereses)
            {
                var interes = await _context.Interest
                    .FirstOrDefaultAsync(i => i.Description == interesNombre);

                if (interes == null)
                {
                    interes = new Interest { Description = interesNombre };
                    _context.Interest.Add(interes);
                    await _context.SaveChangesAsync();
                }

                _context.Set<InterestProfile>().Add(new InterestProfile
                {
                    Profile_Id = profile.Id,
                    Interest_Id = interes.Id
                });
            }

            await _context.SaveChangesAsync();

            return user.Id;
        }

        internal async Task<Usuario?> ValidarCredencialesAsync(string? username, string? password)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}
