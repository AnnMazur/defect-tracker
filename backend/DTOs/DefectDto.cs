using System;

namespace defectTracker.DTOs
{
    public class DefectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "New";        // New, InProgress, InReview, Closed, Cancelled
        public string Priority { get; set; } = "Normal";   // Low, Normal, High
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }             // срок (если нужен — синхронизировать с моделью)
        public Guid ProjectId { get; set; }
        public ProjectDto? Project { get; set; }
        public Guid CreatedById { get; set; }
        public UserDto? CreatedBy { get; set; }
        public Guid? AssignedToId { get; set; }
        public UserDto? AssignedTo { get; set; }
    }
}