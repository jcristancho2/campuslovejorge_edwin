using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Administradores.Application.Services;
using campuslovejorge_edwin.Src.Modules.Administradores.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Shared.Configuration;
using campuslovejorge_edwin.Src.Shared.Context;
using Pomelo.EntityFrameworkCore.MySql.Query.Expressions.Internal;

namespace campuslovejorge_edwin.Src.Modules.Administradores.UI
{
    public class MenuAdministrador 
    {
        private readonly AppDbContext _context;
        readonly AdministradorRepository repo = null!;
        readonly AdministradorService service = null!;
        public MenuAdministrador(AppDbContext context)
        {
            _context = context;
            repo = new AdministradorRepository(_context);
            service = new AdministradorService(repo);
        }
        public async Task RenderMenu()
        {
            bool salir = false;
            while (!salir)
            {


                Console.WriteLine("Menu de Administrador");
                Console.WriteLine("1. Registrar Usuario");
                Console.WriteLine("2. Iniciar sesion ");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");
                int opcion = int.Parse(Console.ReadLine()!);

                switch (opcion)
                {
                    case 1:
                        await AgregarAdministrador();
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        await EditarAdministrador();
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();

                        break;
                    case 3:
                        await EliminarAdministrador();
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        await IniciarSesionAdmin();
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
             }


        }
        private async Task AgregarAdministrador()
        {
            Console.Write("Ingrese tu nombre: ");
            var name = Console.ReadLine();
            Console.Write("Ingrese tu apellido: ");
            var lastname = Console.ReadLine();
            Console.Write("Ingrese tu identificación: ");
            var identification = Console.ReadLine();
            Console.Write("Ingrese tu nombre de usuario: ");
            var username = Console.ReadLine();
            Console.Write("Ingrese tu contraseña: ");
            var password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastname) || string.IsNullOrWhiteSpace(identification) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Todos los campos son obligatorios.");
                return;
            }
            try
            {
                await service.RegistrarAdministradorAsync(name!, lastname!, identification!, username!, password!);
                Console.WriteLine("Administrador agregado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async Task EditarAdministrador()
        {
            Console.Write("Ingrese el ID del administrador a editar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }
            Console.Write("Ingrese el nuevo nombre: ");
            var newname = Console.ReadLine();
            Console.Write("Ingrese el nuevo apellido: ");
            var newlastname = Console.ReadLine();
            Console.Write("Ingrese la nueva identificación: ");
            var newidentification = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newname) || string.IsNullOrWhiteSpace(newlastname) || string.IsNullOrWhiteSpace(newidentification))
            {
                Console.WriteLine("Todos los campos son obligatorios.");
                return;
            }
            try
            {
                await service.ActualizarAdministrador(id, newname!, newlastname!, newidentification!);
                Console.WriteLine("Administrador actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
        private async Task EliminarAdministrador()
        {
            Console.Write("Ingrese el ID del administrador a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }
            try
            {
                await service.EliminarAdministrador(id);
                Console.WriteLine("Administrador eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
        private async Task IniciarSesionAdmin()
        {
            Console.Write("Ingrese su nombre de usuario: ");
            var username = Console.ReadLine();
            Console.Write("Ingrese su contraseña: ");
            var password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Todos los campos son obligatorios.");
                return;
            }
            try
            {
                var administrado = await repo.GetAllAsync();
                var admin = administrado.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (admin != null)
                {
                    Console.WriteLine($"Bienvenido, {admin.Name} {admin.Lastname}");
                    // Aquí puedes agregar más funcionalidades para el administrador
                }
                else
                {
                    Console.WriteLine("Nombre de usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}