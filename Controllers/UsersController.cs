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
using System.Text.Json;

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
                var errorList = ModelState.Values.SelectMany(e => e.Errors).ToList();
                var errormsgs = new List<string>();

                errorList.ForEach(e =>
                {
                    errormsgs.Add(e.ErrorMessage);
                });

                var resultError = new { success = false, errors = errormsgs };

                BadRequest(resultError);
            }

            var userDB = await _context.Users
                .FirstOrDefaultAsync(c => c.Username == request.Username);

            if (userDB != null) return BadRequest("User already exists!");

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
            var result = new { success = true, client = request };
            return Ok(JsonSerializer.Serialize(result));
        }

    }
}