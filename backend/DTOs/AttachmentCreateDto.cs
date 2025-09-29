using Microsoft.AspNetCore.Http;

namespace defectTracker.DTOs
{
    public class AttachmentCreateDto
    {
        public Guid DefectId { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}