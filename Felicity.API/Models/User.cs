namespace Felicity.API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //public ICollection<Team> Teams{get;set;} team or group dashboard if this user is admin?
        //controls over the team settings.
    }
}
