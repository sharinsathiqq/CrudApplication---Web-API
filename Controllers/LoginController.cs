using CrudApplication.Database;
using CrudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CrudApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly StudentDBContext _context;

        public LoginController(StudentDBContext context)
        {
            _context = context;
        }

     

        // POST: api/Login/Authenticate
        [HttpPost("Authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] Login login)
        {
            var existingUser = await _context.Admins.FirstOrDefaultAsync(l => l.Name == login.Username);

            if (existingUser == null || existingUser.Password != login.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            // You can return a JWT token here or some other kind of token in a real application.
            return Ok("Login successful");
        }


        
        }
    }

