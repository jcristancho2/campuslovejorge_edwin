using System;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities
{
    public class DailyLikeLimit
    {
        public int DailyLikeLimitId { get; set; }
        public int UserId { get; set; }
        public int LikesUsed { get; set; } = 0;
        public int MaxLikesPerDay { get; set; } = 10; // Límite por defecto
        public DateTime Date { get; set; } = DateTime.Today;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Propiedades calculadas
        public int RemainingLikes => Math.Max(0, MaxLikesPerDay - LikesUsed);
        public bool CanLike => LikesUsed < MaxLikesPerDay;
        
        // Propiedades de navegación
        public UserEntity User { get; set; } = null!;
    }
}
