using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentAsync(CommentCreateDto dto, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CommentDto>> GetByDefectIdAsync(Guid defectId, CancellationToken cancellationToken = default);
    }
}