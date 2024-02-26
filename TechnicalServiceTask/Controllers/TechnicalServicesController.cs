
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechnicalServiceTask.Controllers;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Models;
using TechnicalServiceTask.Services;

[ApiController]
[Route("api/newtechnicalservices")]
public class NewTechnicalServicesController : BaseController
{
    private readonly TechnicalServiceService _technicalServiceService;

    public NewTechnicalServicesController(BaseService baseService, TechnicalServiceService technicalServiceService) : base(baseService)
    {
        _technicalServiceService = technicalServiceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TechnicalServiceViewModel>>> GetTechnicalServices()
    {
        var technicalServices = await _technicalServiceService.GetTechnicalServices();
        return Ok(technicalServices);
    }

    [HttpPost]
    public async Task<ActionResult<TechnicalService>> CreateTechnicalService([FromBody] TechnicalServiceService.TechnicalServiceRequest technicalServiceRequest)
    {
        return await _technicalServiceService.CreateTechnicalService(technicalServiceRequest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTechnicalService(int id, [FromBody] TechnicalService technicalService)
    {
        try
        {
            await _technicalServiceService.UpdateTechnicalService(id, technicalService);
            return NoContent(); 
        }
        catch (ArgumentException ex)
        {
          
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, "Internal Server Error");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTechnicalService(int id)
    {
        try
        {
            await _technicalServiceService.DeleteTechnicalService(id);
            return NoContent(); 
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, "Internal Server Error");
        }
    }


    [HttpGet("report")]
    public IActionResult GetTechnicalServiceReport(
     [FromQuery] int employeeId = 0,
     [FromQuery] string creationDate = null,
     [FromQuery] string blockCode = null,
     [FromQuery] string systemCode = null)
    {
        DateTime? parsedCreationDate = null;

       
        if (!string.IsNullOrEmpty(creationDate) && DateTime.TryParseExact(creationDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            parsedCreationDate = parsedDate;
        }

        var technicalServiceData = _technicalServiceService.GetTechnicalServiceReport(employeeId, parsedCreationDate, blockCode, systemCode);

        return Ok(technicalServiceData);
    }

}
