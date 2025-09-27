using System;
using System.Collections.Generic;

namespace defectTracker.Models
{
public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = ""; // Инженер, Менеджер, Наблюдатель

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}