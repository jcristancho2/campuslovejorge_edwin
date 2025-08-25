using System;
using campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.Interaction.Domain.Entities
{
    public class Match
    {
        public int MatchId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Propiedades de navegación
        public UserEntity User1 { get; set; } = null!;
        public UserEntity User2 { get; set; } = null!;
    }
}
