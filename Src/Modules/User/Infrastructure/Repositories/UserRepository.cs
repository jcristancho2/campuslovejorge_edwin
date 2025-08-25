using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using campuslovejorge_edwin.Src.Shared.Contexts;


namespace campuslovejorge_edwin.Src.Modules.User.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext contexts)
        {
            _context =contexts;
        }
        
        public async Task<List<User>> GretAllAsync()
        {

        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
        var user = await _context.Users.FindAsync(id);
        if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}


