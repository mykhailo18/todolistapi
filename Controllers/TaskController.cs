using System;
using System.Collections.Generic;
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
    public class TaskController : Controller
    {
        private readonly ApiContext _context;

        public TaskController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            User user = await _context.Users.Include(u=>u.Folders).ThenInclude(u=>u.LTasks).FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);
            
            if (user != null)
            {
                Folder folder = user.Folders.FirstOrDefault(f => f.Id == id);
                if (folder == null)
                {
                    return NotFound();
                }
                return Ok(folder.LTasks);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LTaskModel task)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);
            
            if (user != null)
            {
                LTask t = new LTask
                {
                    Color = task.Color,
                    DateTime = task.DateTime,
                    Text = task.Text,
                    FolderId = task.FolderId
                };

                var tas = await _context.LTasks.AddAsync(t);
                await _context.SaveChangesAsync();
                return Created("", tas.Entity);
            }

            return BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            User user = await _context.Users.Include(u=> u.Folders).FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);

            LTask task = await _context.LTasks.Include(u => u.Folder).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (task == null)
            {
                return NotFound();
            }

            if (task.Folder.User.Email != HttpContext.User.Identity.Name)
            {
                return NotFound();
            }

            _context.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }
    }
}