// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
// using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;
// using campuslovejorge_edwin.Src.Shared.Context;

// namespace campuslovejorge_edwin.Src.Modules.User.Infrastructure.Repositories
// {
//     public class CareerRepository : ICareerRepository
//     {
//         private readonly AppDbContext _context;

//         public CareerRepository(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<List<Career>> GetAllCareersAsync()
//         {
//             return await _context.Careers
//                 .OrderBy(c => c.Category)
//                 .ThenBy(c => c.Name)
//                 .ToListAsync();
//         }

//         public async Task<List<Career>> GetCareersByCategoryAsync(string category)
//         {
//             return await _context.Careers
//                 .Where(c => c.Category == category)
//                 .OrderBy(c => c.Name)
//                 .ToListAsync();
//         }

//         public async Task<Career?> GetCareerByIdAsync(int id)
//         {
//             return await _context.Careers.FindAsync(id);
//         }

//         public async Task<Career?> GetCareerByNameAsync(string name)
//         {
//             return await _context.Careers
//                 .FirstOrDefaultAsync(c => c.Name == name);
//         }
//     }
// }
