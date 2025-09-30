using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.Database;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using defectTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace defectTracker.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;

        public AttachmentService(ApplicationDbContext context)
        {
            _context = context;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<AttachmentDto> UploadAsync(Guid defectId, IFormFile file, Guid uploadedBy, CancellationToken cancellationToken = default)
        {
            var defect = await _context.Defects.FindAsync(new object[] { defectId }, cancellationToken);
            if (defect == null) throw new Exception("Defect not found");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            var attachment = new Attachment
            {
                Id = Guid.NewGuid(),
                DefectId = defectId,
                FilePath = fileName,
                UploadedAt = DateTime.UtcNow
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync(cancellationToken);

            return new AttachmentDto
            {
                Id = attachment.Id,
                DefectId = attachment.DefectId,
                FilePath = attachment.FilePath,
                UploadedAt = attachment.UploadedAt
            };
        }

        public async Task DeleteAsync(Guid attachmentId, CancellationToken cancellationToken = default)
        {
            var attachment = await _context.Attachments.FindAsync(new object[] { attachmentId }, cancellationToken);
            if (attachment == null) throw new Exception("Attachment not found");

            var filePath = Path.Combine(_uploadPath, attachment.FilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
