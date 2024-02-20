using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalServiceTask.Models;

[Authorize]
[ApiController]
[Route("api/blocks")]
public class BlocksController : ControllerBase
{
    private readonly AppEntity _context;

    public BlocksController(AppEntity context)
    {
        _context = context;
    }

    // GET: api/blocks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Block>>> GetBlocks()
    {
        return await _context.Blocks.ToListAsync();
    }

    // GET: api/blocks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Block>> GetBlock(int id)
    {
        var block = await _context.Blocks.FindAsync(id);

        if (block == null)
        {
            return NotFound();
        }

        return block;
    }

    // POST: api/blocks
    [HttpPost]
    public async Task<ActionResult<Block>> CreateBlock([FromBody] Block block)
    {
        _context.Blocks.Add(block);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBlock), new { id = block.Id }, block);
    }

    // PUT: api/blocks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlock(int id, [FromBody] Block block)
    {
        if (id != block.Id)
        {
            return BadRequest();
        }

        _context.Entry(block).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Blocks.Any(b => b.Id == id))
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

    // DELETE: api/blocks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlock(int id)
    {
        var block = await _context.Blocks.FindAsync(id);

        if (block == null)
        {
            return NotFound();
        }

        _context.Blocks.Remove(block);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}