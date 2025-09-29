using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;

namespace defectTracker.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginUserDto dto, CancellationToken cancellationToken = default);
        Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default);
    }
}