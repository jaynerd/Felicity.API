using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Felicity.API.Data;
using Felicity.API.Models;
using Felicity.API.Services;

namespace Felicity.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        // GET api/user
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Users.ToListAsync();
            if (data == null)
                return NotFound("Unable to find users");

            return Ok(await _context.Users.ToListAsync());
        }

        // GET api/user/getuserbyid/5
        [HttpGet("getuserbyid/{id}")]
        public async Task<IActionResult> GetUserById(int? id)
        {
            var data = await _context.Users.ToListAsync();
            if (data == null || id == null)
                return NotFound("Please enter a valid id");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return NotFound("Unable to find the requested user");

            return Ok(user);
        }

        // POST api/user/createuser
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody]JObject jObject)
        {
            if (ModelState.IsValid)
            {
                var username = jObject.Value<string>("username");
                var password = jObject.Value<string>("password");

                if (string.IsNullOrWhiteSpace(username))
                    return Ok("Please enter a valid username");

                if (string.IsNullOrWhiteSpace(password))
                    return Ok("Please enter a valid password");

                if (await _context.Users.SingleOrDefaultAsync(u => u.UserName == username) != null)
                    return Ok("Username already taken");

                JObject credentials = SecurityService.EncodePassword(password);

                User user = new User
                {
                    UserName = username,
                    Password = credentials.Value<string>("hash"),
                    Salt = credentials.Value<string>("salt")
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                return Ok("true");
            }

            return Ok("Account registration failed");
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
