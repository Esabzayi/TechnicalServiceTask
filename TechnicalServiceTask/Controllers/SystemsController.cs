using Microsoft.AspNetCore.Mvc;
using TechnicalServiceTask.Controllers;
using TechnicalServiceTask.Models;
using TechnicalServiceTask.Services;


[ApiController]
[Route("api/systems")]
public class SystemsController : BaseController
{
    private readonly SystemService _systemService;

    public SystemsController(BaseService baseService, SystemService systemService) : base(baseService)
    {
        _systemService = systemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemViewModel>>> GetSystems()
    {
        var systems = await _systemService.GetSystemsViewModels();
        return Ok(systems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SystemViewModel>> GetSystem(int id)
    {
        var system = await _systemService.GetSystemViewModelById(id);

        if (system == null)
        {
            return NotFound();
        }

        return system;
    }

    [HttpPost]
    public async Task<ActionResult<SystemViewModel>> CreateSystem([FromBody] SystemViewModel systemViewModel)
    {
        var createdSystem = await _systemService.CreateSystem(systemViewModel);
        return CreatedAtAction(nameof(GetSystem), new { id = createdSystem.Id }, createdSystem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSystem(int id, [FromBody] SystemViewModel systemViewModel)
    {
        await _systemService.UpdateSystem(id, systemViewModel);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSystem(int id)
    {
        await _systemService.DeleteSystem(id);
        return NoContent();
    }
}
