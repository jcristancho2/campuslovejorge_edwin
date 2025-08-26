using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Users.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Users.Application.Interfaces
{
    public interface IUsersService
    {
        Task<int> RegistrarUsuarioAsync(UsuarioRegistroDto dto);
    }
}