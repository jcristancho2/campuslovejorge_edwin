using campuslovejorge_edwin.Src.Shared.Context;
using campuslovejorge_edwin.Src.Modules.User.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using campuslovejorge_edwin.Src.Modules.User.UI;

namespace campuslovejorge_edwin;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Crear DbContext usando la factory
            using var context = DbContextFactory.Create();
            Console.WriteLine("Conexión creada exitosamente con la base de datos!");

            // Crear repositorio y servicio de User
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            // Crear e iniciar el menú de User
            var menuUser = new MenuUser(userService);
            await menuUser.ShowMenuAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al conectar con la base de datos: " + ex.Message);
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}