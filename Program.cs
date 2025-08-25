using campuslovejorge_edwin.Src.Shared.Helpers;

namespace campuslovejorge_edwin;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var context = DbContextFactory.Create();

            Console.WriteLine("Conexion creada");
        }
        catch
        {
            throw new Exception("Error al conectar con la base de datos");
        }
        
        
    }
}