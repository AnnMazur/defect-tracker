using System;
using System.Collections.Generic;

namespace defectTracker.Models
{
public class Attachment
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; } = ""; 
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Guid DefectId { get; set; }
        public Defect Defect { get; set; }
    }
}