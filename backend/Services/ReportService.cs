using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.Database;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace defectTracker.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportDefectsCsvAsync(DefectFilterDto filter, CancellationToken cancellationToken = default)
        {
            var query = _context.Defects.Include(d => d.Project).AsQueryable();

            if (filter.ProjectId.HasValue)
                query = query.Where(d => d.ProjectId == filter.ProjectId);
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(d => d.Status == filter.Status);
            if (!string.IsNullOrEmpty(filter.Priority))
                query = query.Where(d => d.Priority == filter.Priority);

            var defects = await query.ToListAsync(cancellationToken);

            var sb = new StringBuilder();
            sb.AppendLine("Id;Title;Status;Priority;Project;CreatedAt;DueDate");

            foreach (var d in defects)
            {
                sb.AppendLine($"{d.Id};{d.Title};{d.Status};{d.Priority};{d.Project?.Name};{d.CreatedAt:yyyy-MM-dd};{d.DueDate?.ToString("yyyy-MM-dd")}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<object> GetAnalyticsAsync(DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Defects.AsQueryable();

            if (from.HasValue) query = query.Where(d => d.CreatedAt >= from.Value);
            if (to.HasValue) query = query.Where(d => d.CreatedAt <= to.Value);

            var total = await query.CountAsync(cancellationToken);

            var byStatus = await query.GroupBy(d => d.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var byPriority = await query.GroupBy(d => d.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var byProject = await query.Include(d => d.Project)
                .GroupBy(d => d.Project.Name)
                .Select(g => new { Project = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            return new
            {
                TotalDefects = total,
                ByStatus = byStatus,
                ByPriority = byPriority,
                ByProject = byProject
            };
        }
    }
}
