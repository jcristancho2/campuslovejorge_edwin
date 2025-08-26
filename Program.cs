using campuslovejorge_edwin.Src.Modules.Administradores.UI;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.UI;
using campuslovejorge_edwin.Src.Modules.Users.UI;
using campuslovejorge_edwin.Src.Shared.Helpers;

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
    }
}