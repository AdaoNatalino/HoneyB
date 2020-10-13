using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Honeywell_backend.Data;
using Honeywell_backend.Models;
using Honeywell_backend.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Honeywell_backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private HoneywellDB _context;

        public UsersController(HoneywellDB context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] UserSerializer request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(GenerateErrorsDetails(ModelState));
            }
            var userDB = await _context.Users
                .FirstOrDefaultAsync(c => c.Username == request.Username);

            if (userDB != null) 
            {
                ModelState.AddModelError("User", "User already exists!");
                return BadRequest(GenerateErrorsDetails(ModelState));
            }            

            var user = new User();
            user.Name = request.Name;
            user.Username = request.Username;
            user.Address = request.Address;
            user.Password = request.Password;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.IsStaff = request.IsStaff;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var result = new { success = true, user = user };

            return Ok(result);
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUserDetails(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest("User Not Found!");

            var result = new { success = true, user = user };

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()   
        {
            var usersList = await _context.Users.ToListAsync();

            return Ok( new { success = true, users = usersList });
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginSerializer request)
        {
            var user = await _context.Users
                                .Where(
                                    c => c.Username == request.Username
                                    &&
                                    c.Password == request.Password
                                ).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest( new { success = false, data = "Username and password don't match." });
            }

            return Ok(new { success = true, user = new { username = user.Username } });
        }

        private object GenerateErrorsDetails(ModelStateDictionary ModelState)
        {
            var errorList = ModelState.Values.SelectMany(e => e.Errors).ToList();
            var errormsgs = new List<string>();

            errorList.ForEach(e =>
            {
                errormsgs.Add(e.ErrorMessage);
            });

            var resultError = new { success = false, errors = errormsgs };

            return resultError;
        }

    }
}