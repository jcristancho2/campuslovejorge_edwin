using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Users.Domain.Entities
{
    public class Interest
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public ICollection<InterestProfile> Profiles { get; set; } = new List<InterestProfile>();

    }
}