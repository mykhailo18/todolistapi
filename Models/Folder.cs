using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksListAPI.Models
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool Required { get; set; } = false;
        public DateTime TimeCreate { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<LTask> LTasks { get; set; }

        public Folder()
        {
            LTasks = new List<LTask>();
        }
    }
}