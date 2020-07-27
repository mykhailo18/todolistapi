using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TasksListAPI.Models;
using TasksListAPI.Services;

namespace TasksListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApiContext _context;
        private readonly ILoginService _loginService;

        public LoginController(IConfiguration config, ApiContext context, ILoginService loginService)
        {
            _context = context;
            _config = config;
            _loginService = loginService;
        }

        [AllowAnonymous]    
        [HttpPost]    
        public async Task<IActionResult> Login([FromBody]UserModel login)
        {    
            IActionResult response = Unauthorized();    
            var user = await _loginService.AuthenticateUserAsync(login);    
    
            if (user != null)    
            {    
                var tokenString = _loginService.GenerateJSONWebToken(login);    
                response = Ok(new { token = tokenString, expires = Convert.ToInt32(_config["Jwt:ExpiresMinutes"])});    
            }
    
            return response;
        }
    }
}