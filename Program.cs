using campuslovejorge_edwin.Src.Modules.Administradores.UI;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.UI;
using campuslovejorge_edwin.Src.Modules.Users.UI;
using campuslovejorge_edwin.Src.Shared.Helpers;
﻿using campuslovejorge_edwin.Src.Shared.Context;
using campuslovejorge_edwin.Src.Modules.User.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Modules.User.Application.Services;
using campuslovejorge_edwin.Src.Modules.User.UI;

var context = DbContextFactory.Create();


bool salir = false;
while (!salir)
{
    Console.Clear();
    Console.WriteLine("\n--- MENÚ CRUD ---");
    Console.WriteLine("1. Registro  de usuario");
    Console.WriteLine("2. Iniciar sesion");
    Console.WriteLine("3. Salir");
    Console.Write("Opción: ");
    int opm = int.Parse(Console.ReadLine()!);

    switch (opm)
    {
        case 1:
            await new MenuConsole(context).MostrarMenuAsync();
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
            break;
        case 2:
            await new MenuConsole(context).IniciarSesionAsync();
            break;
        case 3:
            salir = true;
            break;
        default:
            Console.WriteLine("❗ Opción inválida.");
            break;
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