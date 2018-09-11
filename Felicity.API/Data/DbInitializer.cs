using System.Linq;
using Felicity.API.Models;

namespace Felicity.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
            new User{UserName="Carson",Password="Alexander"},
            new User{UserName="Meredith",Password="Alonso"},
            new User{UserName="Arturo",Password="Anand"},
            new User{UserName="Gytis",Password="Barzdukas"},
            };

            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
