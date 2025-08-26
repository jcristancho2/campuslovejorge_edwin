using System;

namespace campuslovejorge_edwin.Src.Modules.User.Domain.Entities
{
    public class UserInterest
    {
        public int UserId { get; set; }
        public int InterestId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Propiedades de navegación
        public UserEntity User { get; set; } = null!;
        public Interest Interest { get; set; } = null!;
    }
}
