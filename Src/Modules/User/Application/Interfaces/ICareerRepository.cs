using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.User.Application.Interfaces
{
    public interface ICareerRepository
    {
        Task<List<Career>> GetAllCareersAsync();
        Task<List<Career>> GetCareersByCategoryAsync(string category);
        Task<Career?> GetCareerByIdAsync(int id);
        Task<Career?> GetCareerByNameAsync(string name);
    }
}
