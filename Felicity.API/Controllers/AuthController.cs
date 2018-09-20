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
    public class AuthController : Controller
    {
        readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }

        // GET api/auth
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Users.ToListAsync();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(await _context.Users.ToListAsync());
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]JObject jObject)
        {
            var username = jObject.Value<string>("username");
            var password = jObject.Value<string>("password");

            //if (string.IsNullOrWhiteSpace(username))
            //    return BadRequest("Please enter a valid username");

            //if (string.IsNullOrWhiteSpace(password))
            //    return BadRequest("Please enter a valid password");

            User user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null)
                return Ok("Account not available");

            bool valid = SecurityService.ValidatePassword(user.Password, user.Salt, password);

            if (valid)
                return Ok(true);

            return Ok("Invalid username or password");
        }
    }
}
