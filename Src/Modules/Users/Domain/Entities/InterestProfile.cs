using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.Users.Domain.Entities
{
    public class InterestProfile
    {
    public int Profile_Id { get; set; }
    public Profile? Profile { get; set; }   // 🔹 Propiedad de navegación

    public int Interest_Id { get; set; }
    public Interest? Interest { get; set; }
    }
}