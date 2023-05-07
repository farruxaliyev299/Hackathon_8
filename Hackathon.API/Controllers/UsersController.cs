using Azure;
using Hackathon.Infrastructure.Persistence.Identity.DTOs;
using Hackathon.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;

        private const string TOKEN = "token";

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _accessor = accessor;
        }
        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync(SignInDto loginDto)
        {
            ApplicationUser existUser = await _userManager.FindByEmailAsync(loginDto.Email);
            if (existUser is null) return BadRequest("Email or Password is incorrect"); 
            
            bool result = await _userManager.CheckPasswordAsync(existUser, loginDto.Password);
            if (result is false) return BadRequest("Email or Password is incorrect");
            IList<string> roles = await _userManager.GetRolesAsync(existUser);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, existUser.Id),
                new Claim(ClaimTypes.Name, existUser.UserName),
                new Claim(ClaimTypes.Email, existUser.Email)
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

            SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            DateTime expires = DateTime.Now.AddMinutes(20);

            JwtSecurityToken securityToken = new(
                    issuer: "https://localhost:7261",
                    audience: "https://localhost:7261",
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: expires
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            _accessor?.HttpContext?.Response.Cookies.Append(TOKEN, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });

            return Ok(token);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAsync(SignUpDto registerDto)
        {
            ApplicationUser existUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existUser is not null) return BadRequest("Email is already registered");

            ApplicationUser user = new()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Substring(0, registerDto.Email.IndexOf("@")) + Guid.NewGuid(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            IdentityResult resultCreateUser = await _userManager.CreateAsync(user, registerDto.Password);

            string? description = resultCreateUser.Errors.Select(d => d.Description).FirstOrDefault();
            if (!resultCreateUser.Succeeded) return BadRequest("Problem occurred while creating account");

            IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, "Customer");
            if (!resultAddRole.Succeeded) return BadRequest("Problems Occured");

            return NoContent();
        }
    }
}
