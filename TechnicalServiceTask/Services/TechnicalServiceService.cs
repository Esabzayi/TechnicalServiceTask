using Microsoft.EntityFrameworkCore;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Models;
using System.ComponentModel.DataAnnotations;

namespace TechnicalServiceTask.Services
{
    public class TechnicalServiceService : BaseService
    {
        public TechnicalServiceService(AppEntity dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<TechnicalService>> GetTechnicalServices()
        {
            return await _dbContext.TechnicalServices
                .Include(ts => ts.Blocks)
                .Include(ts => ts.Systems)
                .Include(ts => ts.Employees)
                .ToListAsync();
        }

        public List<TechnicalServiceDto> GetTechnicalServiceReport(
            int employeeId = 0,
            DateTime? parsedCreationDate = null,
            string blockCode = null,
            string systemCode = null)
        {
            var technicalServiceData = _dbContext.TechnicalServices
                .Where(ts =>
                    (employeeId == 0 || _dbContext.ResponsiblePersons.Any(rp => rp.Id == ts.Id && rp.Id == employeeId))
                    && (!parsedCreationDate.HasValue || ts.CreationTime.Date == parsedCreationDate.Value.Date)
                    && (string.IsNullOrEmpty(blockCode) || _dbContext.Blocks.Any(b => b.Id == ts.Id && b.Code == blockCode))
                    && (string.IsNullOrEmpty(systemCode) || _dbContext.Systems.Any(s => s.Id == ts.Id && s.Code == systemCode)))
                .Include(ts => ts.Activities) // Include activities
                .Join(_dbContext.ResponsiblePersons,
                    ts => ts.Id,
                    rp => rp.Id,
                    (ts, rp) => new { TechnicalService = ts, ResponsiblePerson = rp })
                .Join(_dbContext.Blocks,
                    combined => combined.TechnicalService.Id,
                    b => b.Id,
                    (combined, b) => new { combined.TechnicalService, combined.ResponsiblePerson, Block = b })
                .Join(_dbContext.Systems,
                    combined => combined.TechnicalService.Id,
                    s => s.Id,
                    (combined, s) => new TechnicalServiceDto
                    {
                        TechnicalServiceId = combined.TechnicalService.Id,
                        TechnicalServiceName = combined.TechnicalService.Name,
                        TechnicalServiceDescription = combined.TechnicalService.Description,
                        ResponsiblePersonName = $"{combined.ResponsiblePerson.FirstName} {combined.ResponsiblePerson.Surname} {combined.ResponsiblePerson.LastName}",
                        BlockCode = combined.Block.Code,
                        SystemCode = s.Code,
                        Activities = combined.TechnicalService.Activities.ToList()
                    })
                .ToList();

            return technicalServiceData;
        }


        public async Task<TechnicalService> GetTechnicalServiceById(int id)
        {
            return await _dbContext.TechnicalServices
                .Include(ts => ts.Blocks)
                .Include(ts => ts.Systems)
                .Include(ts => ts.Employees)
                .FirstOrDefaultAsync(ts => ts.Id == id);
        }


        public async Task<TechnicalService> CreateTechnicalService(TechnicalServiceRequest technicalServiceRequest)
        {
            var existingBlocks = await _dbContext.Blocks
                .Where(b => technicalServiceRequest.BlockIds.Contains(b.Id))
                .ToListAsync();

            if (existingBlocks.Count != technicalServiceRequest.BlockIds.Count)
            {
                throw new ArgumentException("One or more BlockIds do not exist.", nameof(technicalServiceRequest.BlockIds));
            }

            var existingSystems = await _dbContext.Systems
                .Where(s => technicalServiceRequest.SystemIds.Contains(s.Id))
                .ToListAsync();

            if (existingSystems.Count != technicalServiceRequest.SystemIds.Count)
            {
                throw new ArgumentException("One or more SystemIds do not exist.", nameof(technicalServiceRequest.SystemIds));
            }

            var validActivities = new List<string> { "creation", "confirmation", "approval", "verification" };

            foreach (var activity in technicalServiceRequest.Activities)
            {
                if (!validActivities.Contains(activity.Name.ToLowerInvariant()))
                {
                    throw new ArgumentException("Invalid activity specified.", nameof(technicalServiceRequest.Activities));
                }
            }

            var technicalService = new TechnicalService
            {
                Name = technicalServiceRequest.Name,
                Description = technicalServiceRequest.Description,
                CreationTime = DateTime.Now,
                Activities = technicalServiceRequest.Activities, // Assuming these activities are validated
                BlockIds = technicalServiceRequest.BlockIds,
                SystemIds = technicalServiceRequest.SystemIds,
                EmployeeIds = technicalServiceRequest.EmployeeIds,
            };

            _dbContext.TechnicalServices.Add(technicalService);
            await _dbContext.SaveChangesAsync();

            return technicalService;
        }


        public async Task UpdateTechnicalService(int id, TechnicalService technicalService)
        {
            if (id != technicalService.Id)
            {

                return;
            }

            _dbContext.Entry(technicalService).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.TechnicalServices.Any(ts => ts.Id == id))
                {

                    return;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteTechnicalService(int id)
        {
            var technicalService = await _dbContext.TechnicalServices.FindAsync(id);

            if (technicalService == null)
            {

                return;
            }

            _dbContext.TechnicalServices.Remove(technicalService);
            await _dbContext.SaveChangesAsync();
        }



        public class TechnicalServiceRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<int> BlockIds { get; set; }
            public List<int> SystemIds { get; set; }
            public int EmployeeIds { get; set; }
            public ICollection<Activity> Activities { get; set; }

        }
    }
}
