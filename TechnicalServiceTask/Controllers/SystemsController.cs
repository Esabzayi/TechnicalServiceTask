using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalServiceTask.Models;
[Authorize]
[ApiController]
[Route("api/systems")]
public class SystemsController : ControllerBase
{
    private readonly AppEntity _context;

    public SystemsController(AppEntity context)
    {
        _context = context;
    }

    // GET: api/systems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TechnicalServiceTask.Models.System>>> GetSystems()
    {
        return await _context.Systems.ToListAsync();
    }

    // GET: api/systems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TechnicalServiceTask.Models.System>> GetSystem(int id)
    {
        var system = await _context.Systems.FindAsync(id);

        if (system == null)
        {
            return NotFound();
        }

        return system;
    }

    // POST: api/systems
    [HttpPost]
    public async Task<ActionResult<TechnicalServiceTask.Models.System>> CreateSystem([FromBody] TechnicalServiceTask.Models.System system)
    {
        _context.Systems.Add(system);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSystem), new { id = system.Id }, system);
    }

    // PUT: api/systems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSystem(int id, [FromBody] TechnicalServiceTask.Models.System system)
    {
        if (id != system.Id)
        {
            return BadRequest();
        }

        _context.Entry(system).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Systems.Any(s => s.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/systems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSystem(int id)
    {
        var system = await _context.Systems.FindAsync(id);

        if (system == null)
        {
            return NotFound();
        }

        _context.Systems.Remove(system);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
