using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksListAPI.Models;

namespace TasksListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : Controller
    {
        private readonly ApiContext _context;

        public FolderController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            User user = await _context.Users.Include(u=>u.Folders).FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);
            if (user != null)
            {
                return Ok(user.Folders.OrderBy(f=>f.TimeCreate));
            }
            return BadRequest();
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FolderModel folder)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);

            if (user != null)
            {
                Folder newFolder = new Folder
                {
                    Color = folder.Color,
                    Name = folder.Name,
                    UserId = user.Id
                };

                var fol = await _context.Folders.AddAsync(newFolder);
                
                await _context.SaveChangesAsync();
                return Created("", fol.Entity);
            }

            return BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            User user = await _context.Users.Include(u=> u.Folders).FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);
            Folder folder = user.Folders.FirstOrDefault(f => f.Id == id);
            if (folder == null)
            {
               return NotFound();
            }

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return Ok(folder);
        }
    }
}