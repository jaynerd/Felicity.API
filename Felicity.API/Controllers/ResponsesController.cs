using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Felicity.API.Data;
using Felicity.API.Models;
using Felicity.API.Services;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Felicity.API.Controllers
{
    [Route("api/[controller]")]
    public class ResponsesController : Controller
    {
        readonly DataContext _context;

        public ResponsesController(DataContext context)
        {
            _context = context;
        }

        // GET api/response
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Responses.ToListAsync();

            if (data == null)
                return NotFound("Unable to find any responses");

            return Ok(data);
        }

        // GET api/response/getresponsebyusercodeanddate/
        [HttpGet]
        public async Task<IActionResult> GetResponseByUsercodeAndDate(string usercode, string date)
        {
            if (string.IsNullOrWhiteSpace(usercode))
                return Ok("Please enter valid usercode");
            if (string.IsNullOrWhiteSpace(date))
                return Ok("Please enter valid date");

            IQueryable<Responses> data = _context.Responses.Where(r => r.UserCode == usercode && r.Date == date);

            if (data == null)
                return NotFound("Unable to find any responses");

            List<Responses> responses = new List<Responses>(data);

            return Ok(responses);
        }

        // GET api/response/getresponsebyteamanddate/
        [HttpGet]
        public async Task<IActionResult> GetResponseByTeamAndDate(string teamname, string date)
        {
            if (string.IsNullOrWhiteSpace(teamname))
                return Ok("Please enter valid team name");
            if (string.IsNullOrWhiteSpace(date))
                return Ok("Please enter valid date");

            IQueryable<Responses> data = _context.Responses.Where(r => r.TeamName == teamname && r.Date == date);

            if (data == null)
                return NotFound("Unable to find any responses");

            List<Responses> responses = new List<Responses>(data);

            return Ok(responses);
        }

        // GET api/response/getresponsebyorganizationanddate/
        [HttpGet]
        public async Task<IActionResult> GetResponseByOrganizationAndDate(string organization, string date)
        {
            if (string.IsNullOrWhiteSpace(organization))
                return Ok("Please enter valid organization name");
            if (string.IsNullOrWhiteSpace(date))
                return Ok("Please enter valid date");

            IQueryable<Responses> data = _context.Responses.Where(r => r.Organization == organization && r.Date == date);

            if (data == null)
                return NotFound("Unable to find any responses");

            List<Responses> responses = new List<Responses>(data);

            return Ok(responses);
        }

        // POST api/response/submitresponse
        [HttpPost("submitresponse")]
        public async Task<IActionResult> CreateUser([FromBody]JObject jObject)
        {
            if (ModelState.IsValid)
            {
                var usercode = jObject.Value<string>("usercode");
                var individualresponse = jObject.Value<string>("individualresponse");
                var teamresponse = jObject.Value<string>("teamresponse");
                var date = DateTime.Today.ToString("dd/MM/yyyy");
                var time = DateTime.Now.ToString("HH:mm").ToString();
                Team TeamDetails = null;
                var organization = "";
                var teamname = "";

                if (string.IsNullOrWhiteSpace(usercode))
                    return Ok("Error submitting response: Invalid usercode!");

                teamname = Regex.Split(usercode, "-")[0];  // Assuming the format of usercode as
                                                           // <TeamName>-<employeeid>
                TeamDetails = await _context.Teams.SingleOrDefaultAsync(t => t.TeamName == teamname);

                if ( TeamDetails != null)
                    organization = TeamDetails.Organization;
                else
                    return Ok("Error submitting response: Coulden't finde team name!");

                Responses response = new Responses
                {
                    UserCode = usercode,
                    IndividualResponse = individualresponse,
                    TeamResponse = teamresponse,
                    Date = date,
                    Time = time,
                    Organization = organization,
                    TeamName = teamname
                };

                _context.Add(response);
                await _context.SaveChangesAsync();

                return Ok(true);
            }

            return Ok("Response submission failed");
        }
    }
}
