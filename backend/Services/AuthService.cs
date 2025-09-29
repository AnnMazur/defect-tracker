using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using defectTracker.Models;
using defectTracker.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace defectTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
        {
            // Проверка уникальности email
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
                throw new Exception("Пользователь с таким Email уже существует");

            // Поиск роли
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role, cancellationToken);
            if (role == null)
            {
                role = new Role { Id = Guid.NewGuid(), Name = dto.Role ?? "Engineer" };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = role.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return GenerateAuthResponse(user, role.Name);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Неверный логин или пароль");

            return GenerateAuthResponse(user, user.Role.Name);
        }

        public async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        private AuthResponseDto GenerateAuthResponse(User user, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = roleName
                }
            };
        }
    }
}
