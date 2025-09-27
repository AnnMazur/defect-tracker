using System;
using System.Collections.Generic;

namespace defectTracker.Models
{
    public class User
    {
        public Guid Id { get; set; }            
        public string Name { get; set; } = "";  
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        
        // Навигационные свойства
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Defect> CreatedDefects { get; set; } = new List<Defect>();
        public ICollection<Defect> AssignedDefects { get; set; } = new List<Defect>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}