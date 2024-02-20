using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechnicalServiceTask.Models;
[Authorize]
[ApiController]
[Route("api/technicalservices")]
public class TechnicalServicesController : ControllerBase
{
    private readonly AppEntity _context;

    public TechnicalServicesController(AppEntity context)
    {
        _context = context;
    }

    // GET: api/technicalservices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TechnicalService>>> GetTechnicalServices()
    {
        return await _context.TechnicalServices
            .Include(ts => ts.Blocks)
            .Include(ts => ts.Systems)
            .Include(ts => ts.Employees)
            .ToListAsync();
    }

    // GET: api/technicalservices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TechnicalService>> GetTechnicalService(int id)
    {
        var technicalService = await _context.TechnicalServices
            .Include(ts => ts.Blocks)
            .Include(ts => ts.Systems)
            .Include(ts => ts.Employees)
            .FirstOrDefaultAsync(ts => ts.Id == id);

        if (technicalService == null)
        {
            return NotFound();
        }

        return technicalService;
    }
    // POST: api/technicalservices
    [HttpPost]
    public async Task<ActionResult<TechnicalService>> CreateTechnicalService([FromBody] TechnicalServiceRequest technicalServiceRequest)
    {
       
        var existingBlocks = await _context.Blocks
            .Where(b => technicalServiceRequest.BlockIds.Contains(b.Id))
            .ToListAsync();

        if (existingBlocks.Count != technicalServiceRequest.BlockIds.Count)
        {
            ModelState.AddModelError("BlockIds", "One or more BlockIds do not exist.");
            return BadRequest(ModelState);
        }

  
        var existingSystems = await _context.Systems
            .Where(s => technicalServiceRequest.SystemIds.Contains(s.Id))
            .ToListAsync();

        if (existingSystems.Count != technicalServiceRequest.SystemIds.Count)
        {
            ModelState.AddModelError("SystemIds", "One or more SystemIds do not exist.");
            return BadRequest(ModelState);
        }

     
        var technicalService = new TechnicalService
        {
            Name = technicalServiceRequest.Name,
            Description = technicalServiceRequest.Description,
            CreationTime = DateTime.Now,
            Activities = technicalServiceRequest.Activities,
            BlockIds = technicalServiceRequest.BlockIds,
            SystemIds = technicalServiceRequest.SystemIds,
            EmployeeIds = technicalServiceRequest.EmployeeIds,
        };

        _context.TechnicalServices.Add(technicalService);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTechnicalService), new { id = technicalService.Id }, technicalService);
    }

    //public async Task<ActionResult<TechnicalService>> CreateTechnicalService([FromBody] TechnicalServiceRequest technicalServiceRequest)
    //{
    //    var blocks = await _context.Blocks
    //        .Where(b => technicalServiceRequest.BlockIds.Contains(b.Id))
    //        .ToListAsync();

    //    var systems = await _context.Systems
    //        .Where(s => technicalServiceRequest.SystemIds.Contains(s.Id))
    //        .ToListAsync();


    //    var technicalService = new TechnicalService
    //    {
    //        Name = technicalServiceRequest.Name,
    //        Description = technicalServiceRequest.Description,
    //        CreationTime = DateTime.Now,
    //        Activities = technicalServiceRequest.Activities,
    //        BlockIds = technicalServiceRequest.BlockIds,
    //        SystemIds = technicalServiceRequest.SystemIds,
    //        EmployeeIds = technicalServiceRequest.EmployeeIds,
    //    };

    //    _context.TechnicalServices.Add(technicalService);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(GetTechnicalService), new { id = technicalService.Id }, technicalService);
    //}


    public class TechnicalServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> BlockIds { get; set; }
        public List<int> SystemIds { get; set; }
        public int EmployeeIds { get; set; }
        public ICollection<Activity> Activities { get; set; }
      
    }


  
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTechnicalService(int id, [FromBody] TechnicalService technicalService)
    {
        if (id != technicalService.Id)
        {
            return BadRequest();
        }

        _context.Entry(technicalService).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TechnicalServices.Any(ts => ts.Id == id))
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

    // DELETE: api/technicalservices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTechnicalService(int id)
    {
        var technicalService = await _context.TechnicalServices.FindAsync(id);

        if (technicalService == null)
        {
            return NotFound();
        }

        _context.TechnicalServices.Remove(technicalService);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("report")]
 
    public IActionResult GetTechnicalServiceData(
    [FromQuery] int employeeId = 0,
    [FromQuery] string creationDate = null,
    [FromQuery] string blockCode = null,
    [FromQuery] string systemCode = null)
    {
        DateTime? parsedCreationDate = null;

        // Parse creationDate if provided
        if (!string.IsNullOrEmpty(creationDate) && DateTime.TryParseExact(creationDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            parsedCreationDate = parsedDate;
        }

        var technicalServiceData = _context.TechnicalServices
            .Where(ts =>
                (employeeId == 0 || _context.ResponsiblePersons.Any(rp => rp.Id == ts.Id && rp.Id == employeeId))
                && (!parsedCreationDate.HasValue || ts.CreationTime.Date == parsedCreationDate.Value.Date)
                && (string.IsNullOrEmpty(blockCode) || _context.Blocks.Any(b => b.Id == ts.Id && b.Code == blockCode))
                && (string.IsNullOrEmpty(systemCode) || _context.Systems.Any(s => s.Id == ts.Id && s.Code == systemCode)))
            .Join(_context.ResponsiblePersons,
                ts => ts.Id,
                rp => rp.Id,
                (ts, rp) => new { TechnicalService = ts, ResponsiblePerson = rp })
            .Join(_context.Blocks,
                combined => combined.TechnicalService.Id,
                b => b.Id,
                (combined, b) => new { combined.TechnicalService, combined.ResponsiblePerson, Block = b })
            .Join(_context.Systems,
                combined => combined.TechnicalService.Id,
                s => s.Id,
                (combined, s) => new TechnicalServiceDto
                {
                    TechnicalServiceId = combined.TechnicalService.Id,
                    TechnicalServiceName = combined.TechnicalService.Name,
                    TechnicalServiceDescription = combined.TechnicalService.Description,
                    ResponsiblePersonName = $"{combined.ResponsiblePerson.FirstName} {combined.ResponsiblePerson.LastName}",
                    BlockCode = combined.Block.Code,
                    SystemCode = s.Code
                })
            .ToList();

        return Ok(technicalServiceData);
    }

   
}





