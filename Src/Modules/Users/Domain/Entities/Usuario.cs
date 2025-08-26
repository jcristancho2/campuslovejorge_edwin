using System.ComponentModel.DataAnnotations.Schema;
using campuslovejorge_edwin.Src.Modules.EstadisticasSistema.Domain.Entities;

namespace campuslovejorge_edwin.Src.Modules.User.Domain.Entities;

public class Usuario {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime BirthDate { get; set; }
        [Column("profile_id")]
        public int Profile_Id { get; set; }
        public Profile? Profile { get; set; }
}
