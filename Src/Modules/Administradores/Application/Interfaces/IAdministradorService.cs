using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Administradores.Application.Interfaces
{
    public interface IAdministradorService
    {
        Task RegistrarAdministradorAsync(string name, string lastname, string identification, string username, string password);
        Task ActualizarAdministrador(int id, string newname, string newlastname, string newidentification);
        Task EliminarAdministrador(int id);
        Task<Administrado> ObtenerAdministradorPorIdAsync(int id);

    }
}