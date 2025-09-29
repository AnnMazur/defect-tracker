using System;

namespace defectTracker.DTOs
{
    public class DefectCreateDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Priority { get; set; } = "Normal";
        public Guid ProjectId { get; set; }
        public Guid? AssignedToId { get; set; }
        public DateTime? DueDate { get; set; }
    }
}