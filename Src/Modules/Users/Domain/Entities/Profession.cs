using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Users.Domain.Entities
{
    public class Profession
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public ICollection<Profile> Profiles { get; set; } = new List<Profile>();
    }
}