using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Administrador.Application.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<Administrador> GetByIdAsync(int id);
        Task<IEnumerable<Administrador>> GetAllAsync();
        void Add(Administrador entity);
        void Remove(Administrador entity);
        void Update(Administrador entity);
        Task SaveAsync();
    }
}