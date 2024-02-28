using Microsoft.EntityFrameworkCore;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data;

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

        public class TechnicalServiceReport
        {

            public string TechnicalServiceName { get; set; }
            public string TechnicalServiceDescription { get; set; }
            public List<string> BlockCodes { get; set; }
            public List<string> SystemNames { get; set; }

            public List<string> activity { get; set; }
            public string ResponsiblePersons { get; set; }
            public DateTime CreationTime { get; set; }
        }

        //public List<TechnicalServiceReport> GetTechnicalServiceReport(
        //    int employeeId = 0,
        //    DateTime? parsedCreationDate = null,
        //    string blockCode = null,
        //    string systemCode = null)
        //{
        //    var report = new List<TechnicalServiceReport>();
        //    var BlockCodeList = new List<string>();
        //    var SystemCodeList = new List<string>();
        //    var get_data = _dbContext.TechnicalServices.Where(x => x.EmployeeIds == employeeId).ToList();

        //    foreach (var item in get_data)
        //    {
        //        var get_BlockCode = _dbContext.TechnicalServiceBlocks.Where(x => x.TechnicalServiceId == item.Id).ToList();
        //        foreach (var bc in get_BlockCode)
        //        {
        //             BlockCodeList = _dbContext.Blocks.Where(x => x.Id == bc.BlockId).Select(x=>x.Code).ToList();
        //        }
        //        var get_SystemCode = _dbContext.TechnicalServiceSystems.Where(x => x.TechnicalServiceId == item.Id).ToList();
        //        foreach (var sc in get_SystemCode)
        //        {
        //            SystemCodeList = _dbContext.Systems.Where(x => x.Id == sc.SystemId).Select(x => x.Code).ToList();
        //        }
        //        var get_emp = _dbContext.ResponsiblePersons.Where(x => x.Id == item.EmployeeIds).Select(x=>x).FirstOrDefault();
        //        if (get_emp!=null)
        //        {
        //            report.Add(new TechnicalServiceReport
        //            {
        //                TechnicalServiceName = item.Name,
        //                TechnicalServiceDescription = item.Description,
        //                BlockCodes = BlockCodeList,
        //                SystemNames = SystemCodeList,
        //                ResponsiblePersons = get_emp.FirstName + " " + get_emp.LastName,
        //                CreationTime = item.CreationTime,
        //            });
        //        }

        //    }

        //    return report;
        //}

        public List<TechnicalServiceReport> GetTechnicalServiceReport(int employeeId = 0, DateTime? parsedCreationDate = null, string blockCode = null, string systemCode = null)
        {
            var report = _dbContext.TechnicalServices
                .Where(x => (employeeId == 0 || x.EmployeeIds == employeeId) &&
                            (!parsedCreationDate.HasValue || x.CreationTime.Date == parsedCreationDate.Value.Date) &&
                            (blockCode == null || x.TechnicalServiceBlocks.Any(bc => _dbContext.Blocks.Any(b => b.Id == bc.BlockId && b.Code == blockCode))) &&
                            (systemCode == null || x.TechnicalServiceSystems.Any(sc => _dbContext.Systems.Any(s => s.Id == sc.SystemId && s.Code == systemCode))))
                .Select(item => new TechnicalServiceReport
                {
                    TechnicalServiceName = item.Name,
                    TechnicalServiceDescription = item.Description,
                    BlockCodes = _dbContext.TechnicalServiceBlocks
                        .Where(bc => bc.TechnicalServiceId == item.Id)
                        .Join(_dbContext.Blocks, bc => bc.BlockId, b => b.Id, (bc, b) => b.Code)
                        .ToList(),
                    SystemNames = _dbContext.TechnicalServiceSystems
                        .Where(sc => sc.TechnicalServiceId == item.Id)
                        .Join(_dbContext.Systems, sc => sc.SystemId, s => s.Id, (sc, s) => s.Code)
                        .ToList(),
                    ResponsiblePersons = _dbContext.ResponsiblePersons
                        .Where(rp => rp.Id == item.EmployeeIds)
                        .Select(rp => rp.FirstName + " " + rp.LastName)
                        .FirstOrDefault(),
                    activity = _dbContext.Activities
                    .Where(at => at.TechnicalServiceId == item.Id)
                    .Select(at=>at.Name) .ToList(),

                    CreationTime = item.CreationTime,
                })
                .ToList();

            return report;
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
                EmployeeIds = technicalServiceRequest.EmployeeIds,
                Activities = technicalServiceRequest.Activities
            };

            _dbContext.TechnicalServices.Add(technicalService);
            await _dbContext.SaveChangesAsync();

            foreach (var block in existingBlocks)
            {
                _dbContext.TechnicalServiceBlocks.Add(new TechnicalServiceBlock { TechnicalServiceId = technicalService.Id, BlockId = block.Id });
            }

            foreach (var system in existingSystems)
            {
                _dbContext.TechnicalServiceSystems.Add(new TechnicalServiceSystem { TechnicalServiceId = technicalService.Id, SystemId = system.Id });
            }

            await _dbContext.SaveChangesAsync();


            var jsonSerializerOptions = new JsonSerializerOptions
            {

                ReferenceHandler = ReferenceHandler.Preserve,

            };

            string jsonString = JsonSerializer.Serialize(technicalService, jsonSerializerOptions);

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
