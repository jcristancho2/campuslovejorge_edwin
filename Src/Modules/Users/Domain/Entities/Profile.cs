using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Users.Domain.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Identification { get; set; } = "";
        public int Gender_Id { get; set; }
        public string Slogan { get; set; } = "";
        public int Status_Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int Profession_Id { get; set; }
        public int Total_Likes { get; set; } = 0;

        public Gender? Gender { get; set; }
        public Profession? Profession { get; set; }
        public ICollection<InterestProfile> Interest { get; set; } = new List<InterestProfile>();

    }
}