using System.Collections.Generic;

namespace Felicity.API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
