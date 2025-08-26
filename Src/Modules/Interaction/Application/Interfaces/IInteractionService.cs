using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Application.Interfaces
{
    public interface IInteractionService
    {
        Task<bool> LikeUserAsync(int likerId, int likedId);
        Task<bool> DislikeUserAsync(int likerId, int dislikedId);
        Task<bool> HasLikedAsync(int likerId, int likedId);
        Task<List<UserEntity>> GetProfilesToReviewAsync(int currentUserId, int limit = 10);
        Task<int> GetProfilesToReviewCountAsync(int currentUserId);
        Task<List<UserEntity>> GetMyLikesAsync(int userId);
        Task<List<UserEntity>> GetWhoLikedMeAsync(int userId);
        Task<List<UserEntity>> GetMyMatchesAsync(int userId);
        Task<int> GetLikesSentCountAsync(int userId);
        Task<int> GetLikesReceivedCountAsync(int userId);
        Task<int> GetMatchesCountAsync(int userId);
        
        // Métodos para estadísticas avanzadas
        Task<UserEntity?> GetUserWithMostLikesAsync();
        Task<UserEntity?> GetUserWithMostMatchesAsync();
        Task<double> GetAverageLikesPerUserAsync();
        Task<int> GetUsersWithoutLikesCountAsync();
        Task<List<UserEntity>> GetTopUsersByLikesAsync(int top = 5);
        Task<List<UserEntity>> GetTopUsersByMatchesAsync(int top = 5);
        
        // Métodos para límite de likes diarios
        Task<bool> CanUserLikeAsync(int userId);
        Task<int> GetRemainingLikesAsync(int userId);
        Task<DailyLikeLimit> GetDailyLikeLimitAsync(int userId);
    }
}
