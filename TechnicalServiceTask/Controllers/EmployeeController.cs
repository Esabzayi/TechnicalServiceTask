using Microsoft.AspNetCore.Mvc;
using TechnicalServiceTask.Controllers;
using TechnicalServiceTask.Models;
using TechnicalServiceTask.Services;

[ApiController]
[Route("employees")]
public class EmployeeController : BaseController
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(BaseService baseService, EmployeeService employeeService) : base(baseService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetResponsiblePersons()
    {
        var employees = await _employeeService.GetResponsiblePersonsViewModels();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeViewModel>> GetResponsiblePerson(int id)
    {
        var employee = await _employeeService.GetResponsiblePersonViewModelById(id);

        if (employee == null)
        {
            return NotFound();
        }

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeViewModel>> CreateResponsiblePerson([FromBody] EmployeeViewModel employeeViewModel)
    {
        var createdEmployee = await _employeeService.CreateResponsiblePerson(employeeViewModel);
        return CreatedAtAction(nameof(GetResponsiblePerson), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResponsiblePerson(int id, [FromBody] EmployeeViewModel employeeViewModel)
    {
        await _employeeService.UpdateResponsiblePerson(id, employeeViewModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResponsiblePerson(int id)
    {
        await _employeeService.DeleteResponsiblePerson(id);
        return NoContent();
    }
}
