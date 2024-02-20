using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalServiceTask.Models;
[Authorize]
[ApiController]
[Route("api/responsiblepersons")]
public class ResponsiblePersonsController : ControllerBase
{
    private readonly AppEntity _context;

    public ResponsiblePersonsController(AppEntity context)
    {
        _context = context;
    }

    // GET: api/responsiblepersons
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetResponsiblePersons()
    {
        return await _context.ResponsiblePersons.ToListAsync();
    }

    // GET: api/responsiblepersons/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetResponsiblePerson(int id)
    {
        var responsiblePerson = await _context.ResponsiblePersons.FindAsync(id);

        if (responsiblePerson == null)
        {
            return NotFound();
        }

        return responsiblePerson;
    }

    // POST: api/responsiblepersons
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateResponsiblePerson([FromBody] Employee responsiblePerson)
    {
        _context.ResponsiblePersons.Add(responsiblePerson);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResponsiblePerson), new { id = responsiblePerson.Id }, responsiblePerson);
    }

    // PUT: api/responsiblepersons/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResponsiblePerson(int id, [FromBody] Employee responsiblePerson)
    {
        if (id != responsiblePerson.Id)
        {
            return BadRequest();
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            // Log or print the error messages
            Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
        }
        _context.Entry(responsiblePerson).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ResponsiblePersons.Any(rp => rp.Id == id))
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

    // DELETE: api/responsiblepersons/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResponsiblePerson(int id)
    {
        var responsiblePerson = await _context.ResponsiblePersons.FindAsync(id);

        if (responsiblePerson == null)
        {
            return NotFound();
        }

        _context.ResponsiblePersons.Remove(responsiblePerson);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
