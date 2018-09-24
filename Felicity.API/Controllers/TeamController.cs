using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Felicity.API.Data;
using Felicity.API.Models;
using Felicity.API.Services;
using System.Linq;
using System.Collections.Generic;

namespace Felicity.API.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        readonly DataContext _context;

        public TeamController(DataContext context)
        {
            _context = context;
        }

        // GET api/team
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Teams.ToListAsync();

            if (data == null)
                return NotFound("Unable to find any teams");

            return Ok(await _context.Teams.ToListAsync());
        }

        // GET api/user/getteambyid/5
        [HttpGet("getteambyid/{id}")]
        public async Task<IActionResult> GetTeamById(int? id)
        {
            var data = await _context.Teams.ToListAsync();
            if (data == null || id == null)
                return NotFound("Please enter a valid id");

            var team = await _context.Teams.FirstOrDefaultAsync(u => u.TeamId == id);

            if (team == null)
                return NotFound("Unable to find the requested team");

            return Ok(team);
        }

        // GET api/user/getteambyuserid
        [HttpGet("getteamsbyuserid/{id}")]
        public IActionResult GetTeamsByUserId(int? id)
        {
            var userId = id;
 
            var teams = _context.TeamUser.Where(tu => tu.UserId == userId);

            List<Team> userTeams = new List<Team>();
            foreach (var team in teams)
            {
                var userTeam = _context.Teams.FirstOrDefault(t => t.TeamId == team.TeamId);
                userTeams.Add(userTeam);
            }

            return Ok(userTeams);
        }

        // POST api/team/createteam
        [HttpPost("createteam")]
        public async Task<IActionResult> CreateTeam([FromBody]JObject jObject)
        {
            if (ModelState.IsValid)
            {
                var userId = jObject.Value<int>("userId");
                var teamname = jObject.Value<string>("teamname");

                if (string.IsNullOrWhiteSpace(teamname))
                    return Ok("Please enter a valid team name");

                if (await _context.Teams.SingleOrDefaultAsync(t => t.TeamName == teamname) != null)
                    return Ok("Team name already taken");

                Team team = new Team { TeamName = teamname };
                User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

                _context.Add(team);
                await _context.SaveChangesAsync();

                TeamUser teamUser = new TeamUser
                {
                    Team = team,
                    User = user
                };

                _context.Add(teamUser);
                await _context.SaveChangesAsync();

                return Ok(true);
            }

            return Ok("Team creation failed");
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
