using System;

namespace TasksListAPI.Models
{
    public class LTaskModel
    {
        public Guid FolderId { get; set; }
    
        public string Text { get; set; }
        public string Color { get; set; } 
        public DateTime? DateTime { get; set; }
    }
}