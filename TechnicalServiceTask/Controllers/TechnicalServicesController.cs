using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechnicalServiceTask.Controllers;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Models;
using TechnicalServiceTask.Services;

[ApiController]
[Route("api/technical-services")]
public class NewTechnicalServicesController : BaseController
{
    private readonly TechnicalServiceService _technicalServiceService;

    public NewTechnicalServicesController(BaseService baseService, TechnicalServiceService technicalServiceService) : base(baseService)
    {
        _technicalServiceService = technicalServiceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTechnicalService([FromBody] TechnicalServiceViewModel model)
    {
        if (await _technicalServiceService.CreateTechnicalService(model))
        {
            return Ok("TechnicalService created successfully");
        }

        return BadRequest("Failed to create TechnicalService");
    }

    [HttpPut("{technicalServiceId}")]
    public async Task<IActionResult> UpdateTechnicalService(int technicalServiceId, [FromBody] TechnicalServiceViewModel updatedModel)
    {
        if (await _technicalServiceService.UpdateTechnicalService(technicalServiceId, updatedModel))
        {
            return Ok(); 
        }

        return NotFound(); 
    }

    [HttpGet("{technicalServiceId}")]
    public async Task<IActionResult> GetTechnicalService(int technicalServiceId)
    {
        var technicalServiceViewModel = await _technicalServiceService.GetTechnicalServiceViewModelById(technicalServiceId);

        if (technicalServiceViewModel != null)
        {
            return Ok(technicalServiceViewModel); 
        }

        return NotFound(); 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTechnicalServiceViewModels()
    {
        var technicalServiceViewModels = await _technicalServiceService.GetAllTechnicalServiceViewModels();

        return Ok(technicalServiceViewModels); 
    }

    [HttpDelete("{technicalServiceId}")]
    public async Task<IActionResult> DeleteTechnicalService(int technicalServiceId)
    {
        var isDeleted = await _technicalServiceService.DeleteTechnicalService(technicalServiceId);

        if (isDeleted)
        {
            return NoContent(); 
        }

        return NotFound(); 
    }


    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<TechnicalServiceReportViewModel>>> GetTechnicalServiceReport(
       [FromQuery] string responsiblePersonName,
       [FromQuery] List<string?> blockCodes,
       [FromQuery] List<string?> systemCodes,
       [FromQuery] DateTime? creationDate)
    {
        try
        {
            var report = await _technicalServiceService.GetTechnicalServiceReport(
                responsiblePersonName,
                blockCodes,
                systemCodes,
                creationDate);

            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
