using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Modules.Users.Infrastructure.Repositories
{
    public class UsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}