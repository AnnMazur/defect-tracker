using System;

namespace defectTracker.DTOs
{
    public class CommentCreateDto
    {
        public Guid DefectId { get; set; }
        public string Text { get; set; } = "";
    }
}