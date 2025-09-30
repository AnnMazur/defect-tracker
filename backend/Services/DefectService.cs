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
    public class DefectService : IDefectService
    {
        private readonly ApplicationDbContext _context;

        public DefectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DefectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects
                .Include(d => d.Project)
                .Include(d => d.CreatedBy)
                .Include(d => d.AssignedTo)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

            if (defect == null) throw new Exception("Defect not found");

            return MapToDto(defect);
        }

        public async Task<(IEnumerable<DefectDto> Items, int TotalCount)> GetFilteredAsync(
            DefectFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Defects
                .Include(d => d.Project)
                .Include(d => d.CreatedBy)
                .Include(d => d.AssignedTo)
                .AsQueryable();

            if (filter.ProjectId.HasValue)
                query = query.Where(d => d.ProjectId == filter.ProjectId);

            if (filter.AssignedToId.HasValue)
                query = query.Where(d => d.AssignedToId == filter.AssignedToId);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(d => d.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(d => d.Priority == filter.Priority);

            if (filter.CreatedFrom.HasValue)
                query = query.Where(d => d.CreatedAt >= filter.CreatedFrom.Value);

            if (filter.CreatedTo.HasValue)
                query = query.Where(d => d.CreatedAt <= filter.CreatedTo.Value);

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(d => d.Title.Contains(filter.Search) || d.Description.Contains(filter.Search));

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            return (items.Select(MapToDto), totalCount);
        }

        public async Task<DefectDto> CreateAsync(DefectCreateDto dto, Guid createdById, CancellationToken cancellationToken = default)
        {
            var defect = new Defect
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Status = "New",
                Priority = dto.Priority,
                ProjectId = dto.ProjectId,
                CreatedById = createdById,
                AssignedToId = dto.AssignedToId,
                DueDate = dto.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Defects.Add(defect);
            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(defect);
        }

        public async Task<DefectDto> UpdateAsync(Guid id, DefectUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { id }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            if (!string.IsNullOrEmpty(dto.Title)) defect.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) defect.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Status)) defect.Status = dto.Status;
            if (!string.IsNullOrEmpty(dto.Priority)) defect.Priority = dto.Priority;
            if (dto.AssignedToId.HasValue) defect.AssignedToId = dto.AssignedToId;
            if (dto.DueDate.HasValue) defect.DueDate = dto.DueDate;

            defect.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(defect);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { id }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            _context.Defects.Remove(defect);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<DefectDto> AssignToAsync(Guid defectId, Guid assigneeId, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { defectId }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            defect.AssignedToId = assigneeId;
            defect.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(defect);
        }

        public async Task<DefectDto> ChangeStatusAsync(Guid defectId, string newStatus, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { defectId }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            defect.Status = newStatus;
            defect.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(defect);
        }

        private static DefectDto MapToDto(Defect defect)
        {
            return new DefectDto
            {
                Id = defect.Id,
                Title = defect.Title,
                Description = defect.Description,
                Status = defect.Status,
                Priority = defect.Priority,
                CreatedAt = defect.CreatedAt,
                UpdatedAt = defect.UpdatedAt,
                DueDate = defect.DueDate,
                ProjectId = defect.ProjectId,
                Project = defect.Project != null
                    ? new ProjectDto
                    {
                        Id = defect.Project.Id,
                        Name = defect.Project.Name,
                        Description = defect.Project.Description,
                        StartDate = defect.Project.StartDate,
                        EndDate = defect.Project.EndDate
                    }
                    : null,
                CreatedById = defect.CreatedById,
                CreatedBy = defect.CreatedBy != null
                    ? new UserDto
                    {
                        Id = defect.CreatedBy.Id,
                        Name = defect.CreatedBy.Name,
                        Email = defect.CreatedBy.Email,
                        Role = defect.CreatedBy.Role?.Name
                    }
                    : null,
                AssignedToId = defect.AssignedToId,
                AssignedTo = defect.AssignedTo != null
                    ? new UserDto
                    {
                        Id = defect.AssignedTo.Id,
                        Name = defect.AssignedTo.Name,
                        Email = defect.AssignedTo.Email,
                        Role = defect.AssignedTo.Role?.Name
                    }
                    : null
            };
        }
    }
}
