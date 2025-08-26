using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Users.Domain.Entities
{
    public class UsuarioRegistroDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Identificacion { get; set; }
        public int Gender_Id { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? FrasePerfil { get; set; }

        // Aquí cambiamos:
        public string? Profesion { get; set; } 
         public Gender? Gender { get; set; }
        public List<string> Intereses { get; set; } = new();  
    }
}