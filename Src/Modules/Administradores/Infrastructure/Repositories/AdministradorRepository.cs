using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Administradores.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Administradores.Domain.Entities;
using campuslovejorge_edwin.Src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Modules.Administradores.Infrastructure.Repositories
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly AppDbContext _context;

        public AdministradorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Administrado?> GetByIdAsync(int id)
        {
            return await _context.Administrador
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Administrado?>> GetAllAsync()
        {
            return await _context.Administrador.ToListAsync();
        }

        public void Add(Administrado entity)
        {
            _context.Administrador.Add(entity);
        }

        public void Remove(Administrado entity) => _context.Administrador.Remove(entity);

        public void Update(Administrado entity) => _context.Administrador.Update(entity);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}