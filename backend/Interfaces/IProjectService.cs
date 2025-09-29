using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ProjectDto> CreateAsync(ProjectCreateDto dto, CancellationToken cancellationToken = default);
        Task<ProjectDto> UpdateAsync(Guid id, ProjectUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
