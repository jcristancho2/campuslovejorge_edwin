using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.User.Domain.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public int GenderId { get; set; }
        public int OrientationId { get; set; }
        public int CareerId { get; set; } // Cambiado de string a int
        public string ProfilePhrase { get; set; } = string.Empty; // Frase de perfil
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Propiedades calculadas
        public int Age => CalculateAge();
        
        // Propiedades de navegación
        // public Career Career { get; set; } = null!;
        
        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}