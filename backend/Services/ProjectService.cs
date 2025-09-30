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
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null) throw new Exception("Project not found");

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ProjectDto> CreateAsync(ProjectCreateDto dto, CancellationToken cancellationToken = default)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
        }

        public async Task<ProjectDto> UpdateAsync(Guid id, ProjectUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null) throw new Exception("Project not found");

            if (!string.IsNullOrEmpty(dto.Name)) project.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description)) project.Description = dto.Description;
            if (dto.StartDate.HasValue) project.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) project.EndDate = dto.EndDate.Value;

            await _context.SaveChangesAsync(cancellationToken);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null) throw new Exception("Project not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
