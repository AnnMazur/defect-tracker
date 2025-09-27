 using System;
using System.Collections.Generic;

namespace defectTracker.Models
{
 public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Defect> Defects { get; set; } = new List<Defect>();
    }
}