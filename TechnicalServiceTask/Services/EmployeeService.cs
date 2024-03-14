using Microsoft.EntityFrameworkCore;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Exceptions;
using TechnicalServiceTask.Models;

namespace TechnicalServiceTask.Services
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(AppEntity dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetResponsiblePersonsViewModels()
        {
            var employees = await _dbContext.Employees.ToListAsync();
            return employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                Surname = e.Surname,
                LastName = e.LastName,
                PIN = e.PIN
            });
        }

        public async Task<EmployeeViewModel> GetResponsiblePersonViewModelById(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
                return null;

            return new EmployeeViewModel
            {
                FirstName = employee.FirstName,
                Surname = employee.Surname,
                LastName = employee.LastName,
                PIN = employee.PIN
            };
        }

        public async Task<EmployeeViewModel> CreateResponsiblePerson(EmployeeViewModel employeeViewModel)
        {
            var employeeEntity = new Employee
            {
                FirstName = employeeViewModel.FirstName,
                Surname = employeeViewModel.Surname,
                LastName = employeeViewModel.LastName,
                PIN = employeeViewModel.PIN
            };

            _dbContext.Employees.Add(employeeEntity);
            await _dbContext.SaveChangesAsync();

            return new EmployeeViewModel
            {
                Id = employeeViewModel.Id,
                FirstName = employeeEntity.FirstName,
                Surname = employeeEntity.Surname,
                LastName = employeeEntity.LastName,
                PIN = employeeEntity.PIN
            };
        }

        public async Task UpdateResponsiblePerson(int id, EmployeeViewModel employeeViewModel)
        {
            var employeeEntity = await _dbContext.Employees.FindAsync(id);

            if (employeeEntity == null)
                throw new NotFoundException("Responsible person not found");

            string newFullName = $"{employeeEntity.FirstName} {employeeEntity.LastName}";

            
            bool nameUsedInTechnicalService = await _dbContext.TechnicalServices
                .AnyAsync(ts =>
                    ts.CreatePersonNames.Contains(newFullName) ||
                    ts.ConfirmPersonNames.Contains(newFullName) ||
                    ts.ApprovePersonNames.Contains(newFullName) ||
                    ts.VerifyPersonNames.Contains(newFullName)
                );

            if (nameUsedInTechnicalService)
            {
                throw new InvalidOperationException("Cannot change name. It's been used in Technical Service.");
            }
            else
            {
                employeeEntity.FirstName = employeeViewModel.FirstName;
                employeeEntity.Surname = employeeViewModel.Surname;
                employeeEntity.LastName = employeeViewModel.LastName;
                employeeEntity.PIN = employeeViewModel.PIN;

                await _dbContext.SaveChangesAsync();
            }
           
        }

        public async Task DeleteResponsiblePerson(int id)
        {
            var employeeEntity = await _dbContext.Employees.FindAsync(id);

            if (employeeEntity == null)
                throw new NotFoundException("Responsible person not found");

            _dbContext.Employees.Remove(employeeEntity);
            await _dbContext.SaveChangesAsync();
        }
    }

}
