using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Services;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Infrastructure.Repositories;
using campuslovejorge_edwin.Src.Shared.Context;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.UI
{
    public class MenuEstadisticas
    {
        private readonly AppDbContext _context;
        readonly EstadisticasRepository repo = null!;
        readonly EstadisticasService service = null!;
        public MenuEstadisticas(AppDbContext context)
        {
            _context = context;
            repo = new EstadisticasRepository(_context);
            service = new EstadisticasService(repo);
        }
        public async Task MostrarAsync()
        {

            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("===== ESTADÍSTICAS DEL SISTEMA =====");
                Console.WriteLine("1. Usuario con más likes");
                Console.WriteLine("2. Usuario con más matches");
                Console.WriteLine("3. Ranking de usuarios");
                Console.WriteLine("4. Total de likes de hoy");
                Console.WriteLine("5. Volver al menú principal");

                Console.Write("\nSeleccione una opción: ");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine($"👉 {await service.UsuarioConMasLikesAsync()}");
                        break;

                    case "2":
                        Console.WriteLine($"👉 {await service.UsuarioConMasMatchesAsync()}");
                        break;

                    case "3":
                        var ranking = await service.RankingUsuariosAsync();
                        Console.WriteLine("Ranking de usuarios:");
                        foreach (var r in ranking)
                            Console.WriteLine($" - {r.nombre}: {r.likes} likes");
                        break;

                    case "4":
                        Console.WriteLine($"👉 Total likes hoy: {await service.TotalLikesHoyAsync()}");
                        break;

                    case "5":
                        continuar = false;
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}