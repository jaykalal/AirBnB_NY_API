using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AirBnB_NY_API.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login()
        {
            string user = Request.Form["user"];
            string pass = Request.Form["pass"];
            if (user.Equals("user") && pass.Equals("pass"))
            {
                return Authenticate();
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        public IActionResult Authenticate()
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "user")
            };
            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }
    }
}