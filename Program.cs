using campuslovejorge_edwin.Src.Modules.Administradores.UI;
using campuslovejorge_edwin.Src.Shared.Helpers;

var context = DbContextFactory.Create();


bool salir = false;
while (!salir)
{
    Console.Clear();
    Console.WriteLine("\n--- MENÚ CRUD ---");
    Console.WriteLine("1. Administrar Usuarios");
    Console.WriteLine("2. Administrar Paises");
    Console.WriteLine("3. Salir");
    Console.Write("Opción: ");
    int opm = int.Parse(Console.ReadLine()!);

    switch (opm)
    {
        case 1:
            await new MenuAdministrador(context).RenderMenu();
            break;
        case 2:
            // logica de administracion de paises
            break;
        case 3:
            salir = true;
            break;
        default:
            Console.WriteLine("❗ Opción inválida.");
            break;
    }
}