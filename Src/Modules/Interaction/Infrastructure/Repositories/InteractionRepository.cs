using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using campuslovejorge_edwin.Src.Modules.Interaction.Application.Interfaces;
using campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;
using campuslovejorge_edwin.Src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Infrastructure.Repositories
{
    public class InteractionRepository : IInteractionRepository
    {
        private readonly AppDbContext _context;

        public InteractionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLikeAsync(int likerId, int likedId)
        {
            try
            {
                // Verificar que ambos usuarios existan
                var liker = await _context.Users.FindAsync(likerId);
                var liked = await _context.Users.FindAsync(likedId);
                
                if (liker == null || liked == null)
                    return false;

                // Crear el like
                var userLike = new UserLike
                {
                    LikerId = likerId,
                    LikedId = likedId,
                    CreatedAt = DateTime.Now
                };

                await _context.Set<UserLike>().AddAsync(userLike);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveLikeAsync(int likerId, int likedId)
        {
            try
            {
                var like = await _context.Set<UserLike>()
                    .FirstOrDefaultAsync(l => l.LikerId == likerId && l.LikedId == likedId);
                
                if (like == null)
                    return false;

                _context.Set<UserLike>().Remove(like);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasLikedAsync(int likerId, int likedId)
        {
            return await _context.Set<UserLike>()
                .AnyAsync(l => l.LikerId == likerId && l.LikedId == likedId);
        }

        public async Task<List<UserEntity>> GetUsersLikedByAsync(int userId)
        {
            var likedUserIds = await _context.Set<UserLike>()
                .Where(l => l.LikerId == userId)
                .Select(l => l.LikedId)
                .ToListAsync();

            return await _context.Users
                .Where(u => likedUserIds.Contains(u.UserId))
                .ToListAsync();
        }

        public async Task<List<UserEntity>> GetUsersWhoLikedAsync(int userId)
        {
            var likerUserIds = await _context.Set<UserLike>()
                .Where(l => l.LikedId == userId)
                .Select(l => l.LikerId)
                .ToListAsync();

            return await _context.Users
                .Where(u => likerUserIds.Contains(u.UserId))
                .ToListAsync();
        }

        public async Task<Match?> GetMatchAsync(int user1Id, int user2Id)
        {
            return await _context.Set<Match>()
                .FirstOrDefaultAsync(m => 
                    (m.User1Id == user1Id && m.User2Id == user2Id) ||
                    (m.User1Id == user2Id && m.User2Id == user1Id));
        }

        public async Task<List<Match>> GetMatchesForUserAsync(int userId)
        {
            return await _context.Set<Match>()
                .Where(m => m.User1Id == userId || m.User2Id == userId)
                .ToListAsync();
        }

        public async Task<bool> CreateMatchAsync(int user1Id, int user2Id)
        {
            try
            {
                // Verificar que no exista ya un match
                var existingMatch = await GetMatchAsync(user1Id, user2Id);
                if (existingMatch != null)
                    return false;

                var match = new Match
                {
                    User1Id = user1Id,
                    User2Id = user2Id,
                    CreatedAt = DateTime.Now
                };

                await _context.Set<Match>().AddAsync(match);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<UserEntity>> GetProfilesToReviewAsync(int currentUserId, int limit = 10)
        {
            // Obtener usuarios que no han sido vistos (liked o disliked) por el usuario actual
            var viewedUserIds = await _context.Set<UserLike>()
                .Where(l => l.LikerId == currentUserId)
                .Select(l => l.LikedId)
                .ToListAsync();

            // Agregar el usuario actual a la lista de excluidos
            viewedUserIds.Add(currentUserId);

            return await _context.Users
                .Where(u => !viewedUserIds.Contains(u.UserId))
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> GetProfilesToReviewCountAsync(int currentUserId)
        {
            var viewedUserIds = await _context.Set<UserLike>()
                .Where(l => l.LikerId == currentUserId)
                .Select(l => l.LikedId)
                .ToListAsync();

            viewedUserIds.Add(currentUserId);

            return await _context.Users
                .Where(u => !viewedUserIds.Contains(u.UserId))
                .CountAsync();
        }

        public async Task<int> GetLikesSentCountAsync(int userId)
        {
            return await _context.Set<UserLike>()
                .Where(l => l.LikerId == userId)
                .CountAsync();
        }

        public async Task<int> GetLikesReceivedCountAsync(int userId)
        {
            return await _context.Set<UserLike>()
                .Where(l => l.LikedId == userId)
                .CountAsync();
        }

        public async Task<int> GetMatchesCountAsync(int userId)
        {
            return await _context.Set<Match>()
                .Where(m => m.User1Id == userId || m.User2Id == userId)
                .CountAsync();
        }

        // Métodos para límite de likes diarios
        public async Task<bool> CanUserLikeAsync(int userId)
        {
            var today = DateTime.Today;
            var dailyLimit = await _context.Set<DailyLikeLimit>()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);

            if (dailyLimit == null)
            {
                // Crear nuevo registro para hoy
                dailyLimit = new DailyLikeLimit
                {
                    UserId = userId,
                    Date = today,
                    LikesUsed = 0,
                    MaxLikesPerDay = 10
                };
                await _context.Set<DailyLikeLimit>().AddAsync(dailyLimit);
                await _context.SaveChangesAsync();
            }

            return dailyLimit.CanLike;
        }

        public async Task<bool> IncrementDailyLikesAsync(int userId)
        {
            var today = DateTime.Today;
            var dailyLimit = await _context.Set<DailyLikeLimit>()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);

            if (dailyLimit != null)
            {
                dailyLimit.LikesUsed = Math.Min(dailyLimit.LikesUsed + 1, dailyLimit.MaxLikesPerDay);
                dailyLimit.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetRemainingLikesAsync(int userId)
        {
            var today = DateTime.Today;
            var dailyLimit = await _context.Set<DailyLikeLimit>()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);

            if (dailyLimit == null)
                return 10; // Límite por defecto

            return dailyLimit.RemainingLikes;
        }

        public async Task<DailyLikeLimit> GetDailyLikeLimitAsync(int userId)
        {
            var today = DateTime.Today;
            var dailyLimit = await _context.Set<DailyLikeLimit>()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == today);

            if (dailyLimit == null)
            {
                dailyLimit = new DailyLikeLimit
                {
                    UserId = userId,
                    Date = today,
                    LikesUsed = 0,
                    MaxLikesPerDay = 10
                };
            }

            return dailyLimit;
        }

        // Métodos para estadísticas avanzadas con LINQ
        public async Task<UserEntity?> GetUserWithMostLikesAsync()
        {
            var userWithMostLikes = await _context.Set<UserLike>()
                .GroupBy(l => l.LikedId)
                .Select(g => new { UserId = g.Key, LikeCount = g.Count() })
                .OrderByDescending(x => x.LikeCount)
                .FirstOrDefaultAsync();

            if (userWithMostLikes == null)
                return null;

            return await _context.Users.FindAsync(userWithMostLikes.UserId);
        }

        public async Task<UserEntity?> GetUserWithMostMatchesAsync()
        {
            var userWithMostMatches = await _context.Set<Match>()
                .SelectMany(m => new[] { m.User1Id, m.User2Id })
                .GroupBy(id => id)
                .Select(g => new { UserId = g.Key, MatchCount = g.Count() })
                .OrderByDescending(x => x.MatchCount)
                .FirstOrDefaultAsync();

            if (userWithMostMatches == null)
                return null;

            return await _context.Users.FindAsync(userWithMostMatches.UserId);
        }

        public async Task<double> GetAverageLikesPerUserAsync()
        {
            var totalLikes = await _context.Set<UserLike>().CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            if (totalUsers == 0)
                return 0;

            return (double)totalLikes / totalUsers;
        }

        public async Task<int> GetUsersWithoutLikesCountAsync()
        {
            var usersWithLikes = await _context.Set<UserLike>()
                .Select(l => l.LikedId)
                .Distinct()
                .CountAsync();

            var totalUsers = await _context.Users.CountAsync();
            return totalUsers - usersWithLikes;
        }

        public async Task<List<UserEntity>> GetTopUsersByLikesAsync(int top = 5)
        {
            var topUserIds = await _context.Set<UserLike>()
                .GroupBy(l => l.LikedId)
                .Select(g => new { UserId = g.Key, LikeCount = g.Count() })
                .OrderByDescending(x => x.LikeCount)
                .Take(top)
                .Select(x => x.UserId)
                .ToListAsync();

            return await _context.Users
                .Where(u => topUserIds.Contains(u.UserId))
                .ToListAsync();
        }

        public async Task<List<UserEntity>> GetTopUsersByMatchesAsync(int top = 5)
        {
            var topUserIds = await _context.Set<Match>()
                .SelectMany(m => new[] { m.User1Id, m.User2Id })
                .GroupBy(id => id)
                .Select(g => new { UserId = g.Key, MatchCount = g.Count() })
                .OrderByDescending(x => x.MatchCount)
                .Take(top)
                .Select(x => x.UserId)
                .ToListAsync();

            return await _context.Users
                .Where(u => topUserIds.Contains(u.UserId))
                .ToListAsync();
        }
    }
}
