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
    public class DefectsController : ControllerBase
    {
        private readonly IDefectService _defectService;

        public DefectsController(IDefectService defectService)
        {
            _defectService = defectService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var defect = await _defectService.GetByIdAsync(id, cancellationToken);
            return Ok(defect);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] DefectFilterDto filter, CancellationToken cancellationToken)
        {
            var (items, total) = await _defectService.GetFilteredAsync(filter, cancellationToken);
            return Ok(new { total, items });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DefectCreateDto dto, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var defect = await _defectService.CreateAsync(dto, userId, cancellationToken);
            return Ok(defect);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DefectUpdateDto dto, CancellationToken cancellationToken)
        {
            var defect = await _defectService.UpdateAsync(id, dto, cancellationToken);
            return Ok(defect);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _defectService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromQuery] Guid assigneeId, CancellationToken cancellationToken)
        {
            var defect = await _defectService.AssignToAsync(id, assigneeId, cancellationToken);
            return Ok(defect);
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] string status, CancellationToken cancellationToken)
        {
            var defect = await _defectService.ChangeStatusAsync(id, status, cancellationToken);
            return Ok(defect);
        }
    }
}
