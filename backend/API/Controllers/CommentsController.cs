using System;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace defectTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var comment = await _commentService.AddCommentAsync(dto, userId, cancellationToken);
            return Ok(comment);
        }

        [HttpGet("defect/{defectId}")]
        public async Task<IActionResult> GetByDefect(Guid defectId, CancellationToken cancellationToken)
        {
            var comments = await _commentService.GetByDefectIdAsync(defectId, cancellationToken);
            return Ok(comments);
        }
    }
}
