// using System.Collections.Generic;
// using System.Threading.Tasks;
// using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
// using campuslovejorge_edwin.Src.Modules.User.Application.Interfaces;

// namespace campuslovejorge_edwin.Src.Modules.User.Application.Services
// {
//     public class CareerService
//     {
//         private readonly ICareerRepository _careerRepository;

//         public CareerService(ICareerRepository careerRepository)
//         {
//             _context = careerRepository;
//         }

//         public async Task<List<Career>> GetAllCareersAsync()
//         {
//             return await _careerRepository.GetAllCareersAsync();
//         }

//         public async Task<List<Career>> GetCareersByCategoryAsync(string category)
//         {
//             return await _careerRepository.GetCareersByCategoryAsync(category);
//         }

//         public async Task<Career?> GetCareerByIdAsync(int id)
//         {
//             return await _careerRepository.GetCareerByIdAsync(id);
//         }

//         public async Task<Career?> GetCareerByNameAsync(string name)
//         {
//             return await _careerRepository.GetCareerByNameAsync(name);
//         }

//         public async Task<List<string>> GetCareerCategoriesAsync()
//         {
//             var careers = await _careerRepository.GetAllCareersAsync();
//             return careers.Select(c => c.Category).Distinct().OrderBy(c => c).ToList();
//         }
//     }
// }
