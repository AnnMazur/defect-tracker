using System;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace defectTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost("{defectId}")]
        public async Task<IActionResult> Upload(Guid defectId, IFormFile file, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var attachment = await _attachmentService.UploadAsync(defectId, file, userId, cancellationToken);
            return Ok(attachment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _attachmentService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
