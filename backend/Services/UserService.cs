using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.Database;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using defectTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace defectTracker.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null) throw new Exception("User not found");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.Name
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role.Name
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role, cancellationToken);
            if (role == null)
            {
                role = new Role { Id = Guid.NewGuid(), Name = dto.Role };
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

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = role.Name
            };
        }

        public async Task<UserDto> UpdateAsync(Guid id, UserUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null) throw new Exception("User not found");

            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Role))
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role, cancellationToken);
                if (role == null)
                {
                    role = new Role { Id = Guid.NewGuid(), Name = dto.Role };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                user.RoleId = role.Id;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = (await _context.Roles.FindAsync(user.RoleId)).Name
            };
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user == null) throw new Exception("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
