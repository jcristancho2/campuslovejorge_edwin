using campuslovejorge_edwin.Src.Shared.Helpers;
using campuslovejorge_edwin.Src.Shared.Context;
using campuslovejorge_edwin.Src.Modules.User.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Interaction.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Services;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.MainMenu;

namespace campuslovejorge_edwin
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Iniciando Campus Love...");
                
                // Crear DbContext usando la factory
                using var context = DbContextFactory.Create();
                Console.WriteLine("Conexión creada exitosamente con la base de datos!");

                // Crear repositorios
                var userRepository = new UserRepository(context);
                var interactionRepository = new InteractionRepository(context);
                Console.WriteLine("Repositorios creados exitosamente!");

                // Crear servicios
                var authService = new AuthService(userRepository);
                var userService = new UserService(userRepository);
                var interactionService = new InteractionService(interactionRepository);
                Console.WriteLine("Servicios creados exitosamente!");

                // Crear e iniciar el menú principal
                var mainMenu = new MainMenu(authService, userService, interactionService);
                Console.WriteLine("Menú principal creado, iniciando...");
                await mainMenu.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }

            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}