

using campuslovejorge_edwin.Src.Modules.Administradores.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Administradores.Application.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _repo;
        public AdministradorService(IAdministradorRepository repo)
        {
            _repo = repo;
        }
        public async Task RegistrarAdministradorAsync(string name, string lastname, string identification, string username, string password)
        {
            var registrados = await _repo.GetAllAsync();
            if (registrados.Any(a => a.Username == username))
                throw new Exception("El Administrador ya existe");
            var administrado = new Administrado
            {
                Name = name,
                Lastname = lastname,
                Identification = identification,
                Username = username,
                Password = password
            };
            _repo.Add(administrado);
            await _repo.SaveAsync();
        }
        public async Task ActualizarAdministrador(int id, string newname, string newlastname, string newidentification)
        {
            var administrado = await _repo.GetByIdAsync(id);
            if (administrado == null)
                throw new Exception($"Administrador con {id} no encontrado");
            administrado.Name = newname;
            administrado.Lastname = newlastname;
            administrado.Identification = newidentification;
            _repo.Update(administrado);
            await _repo.SaveAsync();
        }
        public async Task EliminarAdministrador(int id)
        {
            var administrado = await _repo.GetByIdAsync(id);
            if (administrado == null)
                throw new Exception($"Administrador con {id} no encontrado");
            _repo.Remove(administrado);
            await _repo.SaveAsync();
        }
        public async Task<Administrado> ObtenerAdministradorPorIdAsync(int id)
        {
            var administrado = await _repo.GetByIdAsync(id);
            if (administrado == null)
                throw new Exception($"Administrador con {id} no encontrado");
            return administrado;
        }


    }
}