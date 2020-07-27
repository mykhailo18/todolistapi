using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TasksListAPI.Models;
using TasksListAPI.Services;

namespace TasksListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly ApiContext _context;
        private readonly ILoginService _loginService;
        private readonly IConfiguration _config;

        public RegisterController(ApiContext context, ILoginService loginService, IConfiguration config)
        {
            _context = context;
            _loginService = loginService;
            _config = config;
        }
        
        [AllowAnonymous] 
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            if (user == null || user.Password == null || user.Email == null)
            {
                return BadRequest("Некоректні дані");
            }
            
            User u =  await _context.Users.FirstOrDefaultAsync(u=> u.Email == user.Email);
            if (u == null)
            {
                await _context.AddAsync(new User
                {
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    Folders = new List<Folder>
                    {
                            new Folder(){Name = "Today", Color = "white", Required = true},
                            new Folder(){Name = "Tasks", Color = "white", Required = true}
                    }
                });
                await _context.SaveChangesAsync();
                
                var userAuth = await _loginService.AuthenticateUserAsync(user);
                var tokenString = _loginService.GenerateJSONWebToken(user);
                
                return Ok(new {token = tokenString, expires = Convert.ToInt32(_config["Jwt:ExpiresMinutes"])});
            }
            else
            {
                return BadRequest("Даний користувач уже існує");
            }
        }
    }
}