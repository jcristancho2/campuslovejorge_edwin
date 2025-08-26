using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Users.Application.Interfaces
{
    public interface IUsersRepository
    {
        void Add(Profile entity);
    }
}