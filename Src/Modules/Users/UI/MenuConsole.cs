using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Users.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Users.Application.Services;
using campuslovejorge_edwin.Src.Modules.Users.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.Users.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Shared.Context;

namespace campuslovejorge_edwin.Src.Modules.Users.UI
{
    public class MenuConsole
    {
        private readonly AppDbContext _context;
        readonly UsersRepository repo = null!;
        readonly UsersService service = null!;

        public MenuConsole(AppDbContext context)
        {
            _context = context;
            repo = new UsersRepository(_context);
            service = new UsersService(_context);
        }

        public async Task MostrarMenuAsync()
        {
            Console.WriteLine("=== REGISTRO DE USUARIO ===");

            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string? apellido = Console.ReadLine();

            Console.Write("Identificación: ");
            string? identificacion = Console.ReadLine();

            Console.Write("Fecha nacimiento (yyyy-mm-dd): ");
            DateTime birthdate = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Género Id (ejemplo 1=Masculino, 2=Femenino): ");
            int generoId = int.Parse(Console.ReadLine()!);

            // 🔹 Ahora pedimos texto en lugar de Id
            Console.Write("Profesión (ejemplo: Ingeniero, Médico, Abogado): ");
            string? profesionNombre = Console.ReadLine();

            Console.Write("Frase de perfil: ");
            string? slogan = Console.ReadLine();

            Console.Write("Username: ");
            string? username = Console.ReadLine();

            Console.Write("Password: ");
            string? password = Console.ReadLine();

            // 🔹 Intereses como texto separados por coma
            Console.Write("Intereses (separados por coma, ej: Futbol,Viajar,Cine): ");
            var intereses = Console.ReadLine()!
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            var dto = new UsuarioRegistroDto
            {
                Nombre = nombre,
                Apellido = apellido,
                Identificacion = identificacion,
                FechaNacimiento = birthdate,
                Gender_Id = generoId,
                Profesion = profesionNombre,   // 👈 en lugar de ProfessionId
                FrasePerfil = slogan,
                Username = username,
                Password = password,
                Intereses = intereses        // 👈 en lugar de lista de Ids
            };

            int userId = await service.RegistrarUsuarioAsync(dto);

            Console.WriteLine($"✅ Usuario registrado con ID: {userId}");
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        public async Task IniciarSesionAsync()
        {
            Console.WriteLine("=== INICIO DE SESIÓN ===");

            Console.Write("Username: ");
            string? username = Console.ReadLine();

            Console.Write("Password: ");
            string? password = Console.ReadLine();

            var usuario = await service.ValidarCredencialesAsync(username, password);

            if (usuario != null)
            {
                Console.WriteLine($"✅ Bienvenido {usuario.Profile?.Name}!");
            }
            else
            {
                Console.WriteLine("❌ Credenciales inválidas.");
            }

            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
