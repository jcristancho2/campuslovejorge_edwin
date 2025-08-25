using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Interfaces
{
    public interface IEstadisticaService
    {
        Task<string> UsuarioConMasLikesAsync();
        Task<string> UsuarioConMasMatchesAsync();
        Task<List<(string nombre, int likes)>> RankingUsuariosAsync();
        Task<int> TotalLikesHoyAsync();
    }
}