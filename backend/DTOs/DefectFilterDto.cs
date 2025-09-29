using System;

namespace defectTracker.DTOs
{
    public class DefectFilterDto
    {
        public Guid? ProjectId { get; set; }
        public Guid? AssignedToId { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? Search { get; set; } // текстовый поиск по заголовку/описанию
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
