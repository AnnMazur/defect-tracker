using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateAsync(Guid id, UserUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
