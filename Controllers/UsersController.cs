using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honeywell_backend.Data;
using Honeywell_backend.Models;
using Honeywell_backend.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Honeywell_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private HoneywellDB _context;
        public UsersController(HoneywellDB context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appsettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appsettings.Value;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var userIdentity = new IdentityUser()
            {
                UserName = userRegisterViewModel.Email,
                Email = userRegisterViewModel.Email,
                EmailConfirmed = true,
                PhoneNumber = userRegisterViewModel.Phone
            };

            var request = await _userManager.CreateAsync(userIdentity, userRegisterViewModel.Password);

            foreach (var error in request.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            if (!request.Succeeded) return BadRequest(GenerateErrorsDetails(ModelState));

            var user = new User();
            user.Id = Guid.Parse(userIdentity.Id);
            user.Name = userRegisterViewModel.Name;
            user.Address = userRegisterViewModel.Address;
            user.IsStaff = userRegisterViewModel.IsStaff;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var login = new LoginViewModel()
            {
                Username = userRegisterViewModel.Email,
                Password = userRegisterViewModel.Password
            };
            return await Login(login);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (!response.Succeeded) BadRequest("Username and password don't match.");

            var userIdentity = await _signInManager.UserManager.FindByNameAsync(loginViewModel.Username);

            var userIdParsed = Guid.Parse(userIdentity.Id);
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userIdParsed);

            var responseLogin = new UserLoginResponse()
            {
                AccessToken = GenerateTokenEncoded(),
                ExpiresIn = (int)TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserViewModel()
                {
                    Id = userIdParsed,
                    Email = userIdentity.Email,
                    Phone = userIdentity.PhoneNumber,
                    Name = user.Name,                                       
                    Address = user.Address,                    
                    IsStaff = user.IsStaff
                }

            };
            
            return Ok(responseLogin);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllUsers()
        {
            var response = new List<UserViewModel>();

            var usersIdentityList = await _userManager.Users.ToListAsync();            

            foreach (var userIdentity in usersIdentityList)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userIdentity.Id));
               
                response.Add(new UserViewModel
                {
                    Address = user.Address,
                    Name = user.Name,
                    IsStaff = user.IsStaff,
                    
                    Email = userIdentity.Email,
                    Id = Guid.Parse(userIdentity.Id),
                    Phone = userIdentity.PhoneNumber
                });
            }

            return Ok(new { success = true, users = response });
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

        private string GenerateTokenEncoded()
        {
            var tokenJwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

            var token = tokenJwtHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,                
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenJwtHandler.WriteToken(token);
        }
    }
}
