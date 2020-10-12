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

namespace Honeywell_backend.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private HoneywellDB _context;

        public UsersController (HoneywellDB context) {
            _context = context;
        }

        [HttpPost]

        public async Task<ActionResult> CreateUser ([FromBody] UserSerializer request) {
            if (!ModelState.IsValid) {
                var errorList = ModelState.Values.SelectMany (e => e.Errors).ToList ();
                var errormsgs = new List<string> ();

                errorList.ForEach (e => {
                    errormsgs.Add (e.ErrorMessage);
                });
                var result = new { sucess = false, errors = errormsgs };
                BadRequest (result);
            }

            var clientDb = await _context.Users
                                 .FirstOrDefaultAsync(c => c.Username == request.Username);
            return Ok ("ok");
        }
    }
}