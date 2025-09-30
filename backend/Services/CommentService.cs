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
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommentDto> AddCommentAsync(CommentCreateDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { dto.DefectId }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                DefectId = dto.DefectId,
                UserId = userId,
                Text = dto.Text,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return new CommentDto
            {
                Id = comment.Id,
                DefectId = comment.DefectId,
                UserId = comment.UserId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<IEnumerable<CommentDto>> GetByDefectIdAsync(Guid defectId, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.DefectId == defectId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    DefectId = c.DefectId,
                    UserId = c.UserId,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt,
                    User = new UserDto
                    {
                        Id = c.User.Id,
                        Name = c.User.Name,
                        Email = c.User.Email,
                        Role = c.User.Role.Name
                    }
                })
                .ToListAsync(cancellationToken);
        }
    }
}
