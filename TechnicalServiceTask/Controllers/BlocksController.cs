using Microsoft.AspNetCore.Mvc;
using TechnicalServiceTask.Controllers;
using TechnicalServiceTask.Models;
using TechnicalServiceTask.Services;

[ApiController]
[Route("blocks")]
public class BlocksController : BaseController
{
    private readonly BlockService _blockService;

    public BlocksController(BaseService baseService, BlockService blockService) : base(baseService)
    {
        _blockService = blockService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlockViewModel>>> GetBlocks()
    {
        var blocks = await _blockService.GetBlockViewModels();
        return Ok(blocks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlockViewModel>> GetBlock(int id)
    {
        var block = await _blockService.GetBlockViewModelById(id);

        if (block == null)
        {
            return NotFound();
        }

        return block;
    }

    [HttpPost]
    public async Task<ActionResult<BlockViewModel>> CreateBlock([FromBody] BlockViewModel blockViewModel)
    {
        var createdBlock = await _blockService.CreateBlock(blockViewModel);
        return CreatedAtAction(nameof(GetBlock), new { id = createdBlock.Id }, createdBlock);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlock(int id, [FromBody] BlockViewModel blockViewModel)
    {
        await _blockService.UpdateBlock(id, blockViewModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlock(int id)
    {
        await _blockService.DeleteBlock(id);
        return NoContent();
    }
}
