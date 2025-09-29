using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IDefectService
    {
        Task<DefectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<DefectDto> Items, int TotalCount)> GetFilteredAsync(DefectFilterDto filter, CancellationToken cancellationToken = default);
        Task<DefectDto> CreateAsync(DefectCreateDto dto, Guid createdById, CancellationToken cancellationToken = default);
        Task<DefectDto> UpdateAsync(Guid id, DefectUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // дополнительные операции
        Task<DefectDto> AssignToAsync(Guid defectId, Guid assigneeId, CancellationToken cancellationToken = default);
        Task<DefectDto> ChangeStatusAsync(Guid defectId, string newStatus, CancellationToken cancellationToken = default);
    }
}