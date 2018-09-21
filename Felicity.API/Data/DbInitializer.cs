using System.Linq;
using Felicity.API.Models;

namespace Felicity.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var users = new[] {
                new User { UserName = "nyk", Password = "nyk", TeamCount = 3 },
                new User { UserName = "sienna", Password = "sienna", TeamCount = 4 },
                new User { UserName = "oliver", Password = "oliver", TeamCount = 4 },
                new User { UserName = "nishit", Password = "nishit", TeamCount = 4 },
                new User { UserName = "paul", Password = "paul", TeamCount = 4 },
                new User { UserName = "michelle", Password = "michelle", TeamCount = 4 },
                new User { UserName = "hannah", Password = "hannah", TeamCount = 4 },
            };

            foreach (User u in users)
                context.Users.Add(u);
 
            context.SaveChanges();

            var teams = new[] {
                new Team { TeamName = "beggars" },
                new Team { TeamName = "knights" },
                new Team { TeamName = "scholars" }
            };

            foreach (Team t in teams)
                context.Teams.Add(t);

            context.SaveChanges();

            context.AddRange(
                new TeamUser { Team = teams[0], User = users[0] },
                new TeamUser { Team = teams[0], User = users[1] },
                new TeamUser { Team = teams[1], User = users[2] },
                new TeamUser { Team = teams[1], User = users[3] },
                new TeamUser { Team = teams[2], User = users[4] },
                new TeamUser { Team = teams[2], User = users[5] },
                new TeamUser { Team = teams[2], User = users[6] });

            context.SaveChanges();
        }
    }
}
