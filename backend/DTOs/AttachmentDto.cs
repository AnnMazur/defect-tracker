using System;

namespace defectTracker.DTOs
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public string FilePath { get; set; } = "";
        public DateTime UploadedAt { get; set; }
    }
}