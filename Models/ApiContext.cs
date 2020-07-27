using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TasksListAPI.Models
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LTask> LTasks { get; set; }
        public DbSet<Folder> Folders { get; set; }
        
        public ApiContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=database.db");
        }
    }
}