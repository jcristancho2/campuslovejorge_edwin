using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Administrador.Domain.Entities
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Identification { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }

    }
}