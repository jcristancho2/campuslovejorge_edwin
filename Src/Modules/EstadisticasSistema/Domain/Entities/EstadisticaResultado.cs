using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public int Total_Likes { get; set; }
        public ICollection<UsersLikes> LikesRecibidos { get; set; } = new List<UsersLikes>();
        
    }

    public class UsersLikes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Liked_Profile_Id { get; set; }
        public DateTime Like_Date { get; set; }
        public bool Is_Match { get; set; }
        public Profile? LikedProfile { get; set; }
    }
    public class UserMatch
    {
        public int Id { get; set; }  // PK autoincremental

        // Usuarios involucrados en el match
        public string? User1Id { get; set; }
        public string? User2Id { get; set; }

        // Estado del match
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsMatch { get; set; } = false; // true si ambos dieron like

        // Relaciones de navegación (si tienes entidad User)
        public Profile? User1 { get; set; }
        public Profile? User2 { get; set; }
    }
}