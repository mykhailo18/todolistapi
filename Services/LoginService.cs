using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TasksListAPI.Models;

namespace TasksListAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly ApiContext _context;
        
        public LoginService(IConfiguration config, ApiContext context)
        {
            _config = config;
            _context = context;
        }
        
        public async Task<UserModel> AuthenticateUserAsync(UserModel login)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u=> u.Email == login.Email && u.Password == login.Password);
            if (user == null)
            {
                return null;
            }
            return new UserModel{Email = user.Email, Username = user.Username};
        }
        
        public string GenerateJSONWebToken(UserModel user)  
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                _config["Jwt:Issuer"], 
                claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:ExpiresMinutes"])),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }
    }
}