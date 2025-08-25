using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Application.Interfaces
{
    public interface IEstadisticasRepository
    {
        /// Devuelve el perfil del usuario con más likes recibidos.
        Task<Profile?> GetUsuarioConMasLikesAsync();
        /// Devuelve el nombre del usuario y la cantidad de matches que tiene el que más ha logrado.
        Task<(string usuario, int total)> GetUsuarioConMasMatchesAsync();
        /// Devuelve un ranking de usuarios ordenados por la cantidad de likes recibidos.
        Task<List<(string nombre, int likes)>> GetRankingUsuariosAsync();
        /// Devuelve el total de likes recibidos en la fecha actual (hoy).
        Task<int> GetTotalLikesHoyAsync();

    }
}