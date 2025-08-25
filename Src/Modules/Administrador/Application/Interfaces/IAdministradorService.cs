using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Administrador.Application.Interfaces
{
    public interface IAdministradorService
    {
        Task RegistrarAdministradorAsync(string name, string lastname, string identification, string username, string password);
        Task ActualizarAdministrador(int id, string newname, string newlastname, string newidentification);
        Task EliminarAdministrador(int id);
        Task<Administrador> ObtenerUsuarioPorIdAsync(int id);

    }
}