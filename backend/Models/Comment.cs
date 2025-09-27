using System;
using System.Collections.Generic;

namespace defectTracker.Models
{
 public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid DefectId { get; set; }
        public Defect Defect { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}