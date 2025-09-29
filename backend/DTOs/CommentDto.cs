using System;

namespace defectTracker.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public UserDto? User { get; set; }
    }
}