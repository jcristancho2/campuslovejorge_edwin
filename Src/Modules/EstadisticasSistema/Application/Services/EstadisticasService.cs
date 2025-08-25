using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Infrastructure.Repositories;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Services
{
    public class EstadisticasService : IEstadisticaService
    {
        private readonly EstadisticasRepository _repo;

        public EstadisticasService(EstadisticasRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> UsuarioConMasLikesAsync()
        {
            var user = await _repo.GetUsuarioConMasLikesAsync();
            return user != null ? $"{user.Name} con {user.Total_Likes} likes" : "No hay datos";
        }

        public async Task<string> UsuarioConMasMatchesAsync()
        {
            var (usuario, total) = await _repo.GetUsuarioConMasMatchesAsync();
            return $"{usuario} con {total} matches";
        }

        public async Task<List<(string nombre, int likes)>> RankingUsuariosAsync()
        {
            return await _repo.GetRankingUsuariosAsync();
        }

        public async Task<int> TotalLikesHoyAsync()
        {
            return await _repo.GetTotalLikesHoyAsync();
        }
    }
}