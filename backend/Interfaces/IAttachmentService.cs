using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IAttachmentService
    {
        Task<AttachmentDto> UploadAsync(Guid defectId, IFormFile file, Guid uploadedBy, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid attachmentId, CancellationToken cancellationToken = default);
    }
}