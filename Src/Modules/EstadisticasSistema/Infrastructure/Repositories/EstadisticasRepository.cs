using System;
using System.Collections.Generic;
using System.Linq;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;
using campuslovejorge_edwin.Src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Infrastructure.Repositories
{
    public class EstadisticasRepository : IEstadisticasRepository
    {
        private readonly AppDbContext _context;

        public EstadisticasRepository(AppDbContext context)
        {
            _context = context;
        }

        // Usuario con más likes recibidos
        public async Task<Profile?> GetUsuarioConMasLikesAsync()
        {
            return await _context.Profile
                .OrderByDescending(p => p.Total_Likes)
                .FirstOrDefaultAsync();
        }

        // Usuario con más matches (cuenta en UsersLikes donde IsMatch = true)
        public async Task<(string usuario, int total)> GetUsuarioConMasMatchesAsync()
        {
            var result = await _context.UsersLikes
                .Where(l => l.Is_Match)
                .GroupBy(l => l.Liked_Profile_Id)
                .Select(g => new
                {
                    ProfileId = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(g => g.Total)
                .FirstOrDefaultAsync();

            if (result == null) return ("Ninguno", 0);

            var profile = await _context.Profile.FindAsync(result.ProfileId);
            return (profile?.Name ?? "Desconocido", result.Total);
        }

        // Ranking de usuarios por likes
        public async Task<List<(string nombre, int likes)>> GetRankingUsuariosAsync()
        {
            var estadisticas = await _context.Profile
                .Select(u => new
                {
                    Name = u.Name,
                    TotalLikes = u.Total_Likes
                })
            .OrderByDescending(x => x.TotalLikes)
            .ToListAsync();

        return estadisticas.Select(x => (x.Name, x.TotalLikes)).ToList();
        }

        // Total de likes de hoy
        public async Task<int> GetTotalLikesHoyAsync()
        {
            var hoy = DateTime.Today;
            return await _context.UsersLikes
                .Where(l => l.Like_Date.Date == hoy)
                .CountAsync();
        }


    }
}
