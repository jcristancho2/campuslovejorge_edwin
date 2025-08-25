using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;


namespace campuslovejorge_edwin.Src.Modules.Administradores.Application.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<Administrado?> GetByIdAsync(int id);
        Task<IEnumerable<Administrado?>> GetAllAsync();
        void Add(Administrado entity);
        void Remove(Administrado entity);
        void Update(Administrado entity);
        Task SaveAsync();
    }
}