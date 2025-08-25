using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.User.Domain.Entities{

    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int Age { get; set; }
        public int ProfileId { get; set; }
        public string Gender { get; set; }
        public int BonusLikes { get; set; }
        public List<string> Interests { get; set; }
        public int DailyLikesLeft { get; set; }

        public List<Profile> Profile { get; set; } = new List<Profile>();
    }

}
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Gender { get; set; }
    public string Career { get; set; }
    public string ProfilePhrase { get; set; }
    