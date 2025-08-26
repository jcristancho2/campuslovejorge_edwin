using System;

namespace campuslovejorge_edwin.Src.Modules.User.Domain.Entities
{
    public class Interest
    {
        public int InterestId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Deportes, Música, Arte, etc.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
