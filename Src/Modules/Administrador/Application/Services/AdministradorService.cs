using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Administrador.Application.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorService _repo;
        public AdministradorService(IAdministradorService _repo)
        {
            _repo = repo;
        }
        public async Task RegistrarAdministradorAsync(string name, string lastname, string identification, string username, string password)
        {
            var registrados = await_repo.GetAllAsync();
            if (registrados.Any(a => a.Username == username))
                throw new Exception("El Administrador ya existe");
            var administrador = new Administrador
            {
                Name = nombre,
                Lastname = lastname,
                identification = identification,
                username = username,
                password = password
            };
            _repo.Add(administrador);
            _repo.Update(administrador);
        }
        public async Task ActualizarAdministrador(int id, string newname, string newlastname, string newidentification)
        {
            var administrador = await_repo.GetByIdAsync(id);
            if (administrador == null)
                throw new Exception($"Administrador con {id} no encontrado");
            
        }
    }
}