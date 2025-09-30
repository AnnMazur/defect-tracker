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
    [Authorize] // все авторизованные могут работать с проектами
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetAllAsync(cancellationToken);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(id, cancellationToken);
            return Ok(project);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")] // только менеджеры создают проекты
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto, CancellationToken cancellationToken)
        {
            var project = await _projectService.CreateAsync(dto, cancellationToken);
            return Ok(project);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")] // только менеджеры редактируют
        public async Task<IActionResult> Update(Guid id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken)
        {
            var project = await _projectService.UpdateAsync(id, dto, cancellationToken);
            return Ok(project);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")] // только менеджеры удаляют
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _projectService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
