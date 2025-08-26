using System;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities
{
    public class UserLike
    {
        public int LikeId { get; set; }
        public int LikerId { get; set; }
        public int LikedId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Propiedades de navegación
        public UserEntity Liker { get; set; } = null!;
        public UserEntity Liked { get; set; } = null!;
    }
}
