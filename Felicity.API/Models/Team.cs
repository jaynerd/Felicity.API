using System.Collections.Generic;

namespace Felicity.API.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
    }
}
