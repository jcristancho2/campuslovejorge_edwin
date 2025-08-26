using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Application.Interfaces
{
    public interface IInteractionRepository
    {
        // Métodos para likes
        Task<bool> AddLikeAsync(int likerId, int likedId);
        Task<bool> RemoveLikeAsync(int likerId, int likedId);
        Task<bool> HasLikedAsync(int likerId, int likedId);
        Task<List<UserEntity>> GetUsersLikedByAsync(int userId);
        Task<List<UserEntity>> GetUsersWhoLikedAsync(int userId);
        
        // Métodos para matches
        Task<Match?> GetMatchAsync(int user1Id, int user2Id);
        Task<List<Match>> GetMatchesForUserAsync(int userId);
        Task<bool> CreateMatchAsync(int user1Id, int user2Id);
        
        // Métodos para obtener perfiles para revisar
        Task<List<UserEntity>> GetProfilesToReviewAsync(int currentUserId, int limit = 10);
        Task<int> GetProfilesToReviewCountAsync(int currentUserId);
        
        // Métodos para estadísticas
        Task<int> GetLikesSentCountAsync(int userId);
        Task<int> GetLikesReceivedCountAsync(int userId);
        Task<int> GetMatchesCountAsync(int userId);
        
        // Métodos para límite de likes diarios
        Task<bool> CanUserLikeAsync(int userId);
        Task<bool> IncrementDailyLikesAsync(int userId);
        Task<int> GetRemainingLikesAsync(int userId);
        Task<DailyLikeLimit> GetDailyLikeLimitAsync(int userId);
        
        // Métodos para estadísticas avanzadas con LINQ
        Task<UserEntity?> GetUserWithMostLikesAsync();
        Task<UserEntity?> GetUserWithMostMatchesAsync();
        Task<double> GetAverageLikesPerUserAsync();
        Task<int> GetUsersWithoutLikesCountAsync();
        Task<List<UserEntity>> GetTopUsersByLikesAsync(int top = 5);
        Task<List<UserEntity>> GetTopUsersByMatchesAsync(int top = 5);
    }
}
