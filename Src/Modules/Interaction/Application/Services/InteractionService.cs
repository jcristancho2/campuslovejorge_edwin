using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Application.Services
{
    public class InteractionService
    {
        private readonly IInteractionRepository _repository;

        public InteractionService(IInteractionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> LikeUserAsync(int likerId, int likedId)
        {
            if (likerId == likedId)
                throw new InvalidOperationException("No puedes darte like a ti mismo");

            // Verificar si el usuario puede dar like (límite diario)
            if (!await _repository.CanUserLikeAsync(likerId))
                throw new InvalidOperationException("Has alcanzado el límite de likes diarios");

            // Verificar si ya existe un like
            if (await _repository.HasLikedAsync(likerId, likedId))
                return false;

            // Agregar el like
            var likeAdded = await _repository.AddLikeAsync(likerId, likedId);
            
            if (likeAdded)
            {
                // Incrementar el contador de likes diarios
                await _repository.IncrementDailyLikesAsync(likerId);
                
                // Verificar si hay match (si el otro usuario también dio like)
                if (await _repository.HasLikedAsync(likedId, likerId))
                {
                    await _repository.CreateMatchAsync(likerId, likedId);
                }
            }

            return likeAdded;
        }

        public async Task<bool> DislikeUserAsync(int dislikerId, int dislikedId)
        {
            if (dislikerId == dislikedId)
                throw new InvalidOperationException("No puedes darte dislike a ti mismo");

            // Remover el like si existe
            return await _repository.RemoveLikeAsync(dislikerId, dislikedId);
        }

        public async Task<List<UserEntity>> GetProfilesToReviewAsync(int currentUserId, int limit = 10)
        {
            return await _repository.GetProfilesToReviewAsync(currentUserId, limit);
        }

        public async Task<int> GetProfilesToReviewCountAsync(int currentUserId)
        {
            return await _repository.GetProfilesToReviewCountAsync(currentUserId);
        }

        public async Task<List<Match>> GetMatchesAsync(int userId)
        {
            return await _repository.GetMatchesForUserAsync(userId);
        }

        public async Task<List<UserEntity>> GetUsersLikedByAsync(int userId)
        {
            return await _repository.GetUsersLikedByAsync(userId);
        }

        public async Task<List<UserEntity>> GetUsersWhoLikedAsync(int userId)
        {
            return await _repository.GetUsersWhoLikedAsync(userId);
        }

        public async Task<bool> HasLikedAsync(int likerId, int likedId)
        {
            return await _repository.HasLikedAsync(likerId, likedId);
        }

        public async Task<Match?> GetMatchAsync(int user1Id, int user2Id)
        {
            return await _repository.GetMatchAsync(user1Id, user2Id);
        }

        // Métodos para estadísticas
        public async Task<int> GetLikesSentCountAsync(int userId)
        {
            return await _repository.GetLikesSentCountAsync(userId);
        }

        public async Task<int> GetLikesReceivedCountAsync(int userId)
        {
            return await _repository.GetLikesReceivedCountAsync(userId);
        }

        public async Task<int> GetMatchesCountAsync(int userId)
        {
            return await _repository.GetMatchesCountAsync(userId);
        }

        // Métodos para estadísticas avanzadas
        public async Task<UserEntity?> GetUserWithMostLikesAsync()
        {
            return await _repository.GetUserWithMostLikesAsync();
        }

        public async Task<UserEntity?> GetUserWithMostMatchesAsync()
        {
            return await _repository.GetUserWithMostMatchesAsync();
        }

        public async Task<double> GetAverageLikesPerUserAsync()
        {
            return await _repository.GetAverageLikesPerUserAsync();
        }

        public async Task<int> GetUsersWithoutLikesCountAsync()
        {
            return await _repository.GetUsersWithoutLikesCountAsync();
        }

        public async Task<List<UserEntity>> GetTopUsersByLikesAsync(int top = 5)
        {
            return await _repository.GetTopUsersByLikesAsync(top);
        }

        public async Task<List<UserEntity>> GetTopUsersByMatchesAsync(int top = 5)
        {
            return await _repository.GetTopUsersByMatchesAsync(top);
        }

        // Métodos para límite de likes diarios
        public async Task<bool> CanUserLikeAsync(int userId)
        {
            return await _repository.CanUserLikeAsync(userId);
        }

        public async Task<int> GetRemainingLikesAsync(int userId)
        {
            return await _repository.GetRemainingLikesAsync(userId);
        }

        public async Task<DailyLikeLimit> GetDailyLikeLimitAsync(int userId)
        {
            return await _repository.GetDailyLikeLimitAsync(userId);
        }
    }
}
