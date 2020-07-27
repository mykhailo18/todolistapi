using System;
using System.Text.Json.Serialization;

namespace TasksListAPI.Models
{
    public class LTask
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public DateTime? DateTime { get; set; }
        
        [JsonIgnore]
        public Guid FolderId { get; set; }
        
        [JsonIgnore]
        public Folder Folder { get; set; }
    }
}