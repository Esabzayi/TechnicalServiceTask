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
        public async Task<bool> CreateTechnicalService(TechnicalServiceViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            var createPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == model.CreatePersonId);
            var confirmPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == model.ConfirmPersonId);
            var approvePerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == model.ApprovePersonId);
            var verifyPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == model.VerifyPersonId);

            if (createPerson == null || confirmPerson == null || approvePerson == null || verifyPerson == null)
            {
                return false;
            }

            var technicalService = new TechnicalService
            {
                Name = model.Name,
                Description = model.Description,
                CreationTime = DateTime.Now,
                CreatePersonId = createPerson.Id,
                CreatePersonNames = $"{createPerson.FirstName} {createPerson.LastName}",
                ConfirmPersonId = confirmPerson.Id,
                ConfirmPersonNames = $"{confirmPerson.FirstName} {confirmPerson.LastName}",
                ApprovePersonId = approvePerson.Id,
                ApprovePersonNames = $"{approvePerson.FirstName} {approvePerson.LastName}",
                VerifyPersonId = verifyPerson.Id,
                VerifyPersonNames = $"{verifyPerson.FirstName} {verifyPerson.LastName}",
            };
          
            _dbContext.TechnicalServices.Add(technicalService);
            await _dbContext.SaveChangesAsync();
        
            var existingBlocks = await _dbContext.Blocks.Where(b => model.BlockIds.Contains(b.Id)).ToListAsync();
            var existingSystems = await _dbContext.Systems.Where(s => model.SystemIds.Contains(s.Id)).ToListAsync();
          
            foreach (var block in existingBlocks)
            {
                _dbContext.TechnicalServiceBlocks.Add(new TechnicalServiceBlock { TechnicalServiceId = technicalService.Id, BlockId = block.Id });
            }
          
            foreach (var system in existingSystems)
            {
                _dbContext.TechnicalServiceSystems.Add(new TechnicalServiceSystem { TechnicalServiceId = technicalService.Id, SystemId = system.Id });
            }
           
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTechnicalService(int technicalServiceId, TechnicalServiceViewModel updatedModel)
        {
            if (updatedModel == null)
            {
                return false;
            }

            var technicalService = await _dbContext.TechnicalServices
                .FirstOrDefaultAsync(ts => ts.Id == technicalServiceId);

            if (technicalService == null)
            {
                return false; 
            }

            var createPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == updatedModel.CreatePersonId);
            var confirmPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == updatedModel.ConfirmPersonId);
            var approvePerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == updatedModel.ApprovePersonId);
            var verifyPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == updatedModel.VerifyPersonId);

            if (createPerson == null || confirmPerson == null || approvePerson == null || verifyPerson == null)
            {
                return false;
            }
       
            technicalService.Name = updatedModel.Name;
            technicalService.Description = updatedModel.Description;
            technicalService.CreatePersonId = createPerson.Id;
            technicalService.CreatePersonNames = $"{createPerson.FirstName} {createPerson.LastName}";
            technicalService.ConfirmPersonId = confirmPerson.Id;
            technicalService.ConfirmPersonNames = $"{confirmPerson.FirstName} {confirmPerson.LastName}";
            technicalService.ApprovePersonId = approvePerson.Id;
            technicalService.ApprovePersonNames = $"{approvePerson.FirstName} {approvePerson.LastName}";
            technicalService.VerifyPersonId = verifyPerson.Id;
            technicalService.VerifyPersonNames = $"{verifyPerson.FirstName} {verifyPerson.LastName}";
           
            await UpdateTechnicalServiceBlocks(technicalService, updatedModel.BlockIds);
            await UpdateTechnicalServiceSystems(technicalService, updatedModel.SystemIds);
           
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private async Task UpdateTechnicalServiceBlocks(TechnicalService technicalService, List<int> blockIds)
        {
            
            var existingBlocks = await _dbContext.TechnicalServiceBlocks
                .Where(tsb => tsb.TechnicalServiceId == technicalService.Id)
                .ToListAsync();

            _dbContext.TechnicalServiceBlocks.RemoveRange(existingBlocks);

            
            foreach (var blockId in blockIds)
            {
                _dbContext.TechnicalServiceBlocks.Add(new TechnicalServiceBlock { TechnicalServiceId = technicalService.Id, BlockId = blockId });
            }
        }
        private async Task UpdateTechnicalServiceSystems(TechnicalService technicalService, List<int> systemIds)
        {
            
            var existingSystems = await _dbContext.TechnicalServiceSystems
                .Where(tss => tss.TechnicalServiceId == technicalService.Id)
                .ToListAsync();

            _dbContext.TechnicalServiceSystems.RemoveRange(existingSystems);

           
            foreach (var systemId in systemIds)
            {
                _dbContext.TechnicalServiceSystems.Add(new TechnicalServiceSystem { TechnicalServiceId = technicalService.Id, SystemId = systemId });
            }
        }
        public async Task<IEnumerable<TechnicalServiceViewModel>> GetAllTechnicalServiceViewModels()
        {
            return await _dbContext.TechnicalServices
                .Select(ts => new TechnicalServiceViewModel
                {
                    Name = ts.Name,
                    Description = ts.Description,
                    CreatePersonId = ts.CreatePersonId,
                    ConfirmPersonId = ts.ConfirmPersonId,
                    ApprovePersonId = ts.ApprovePersonId,
                    VerifyPersonId = ts.VerifyPersonId,
                    BlockIds = ts.TechnicalServiceBlocks.Select(tsb => tsb.BlockId).ToList(),
                    SystemIds = ts.TechnicalServiceSystems.Select(tss => tss.SystemId).ToList()
                })
                .ToListAsync();
        }
        public async Task<TechnicalServiceViewModel> GetTechnicalServiceViewModelById(int technicalServiceId)
        {
            return await _dbContext.TechnicalServices
                .Where(ts => ts.Id == technicalServiceId)
                .Select(ts => new TechnicalServiceViewModel
                {
                    Name = ts.Name,
                    Description = ts.Description,
                    CreatePersonId = ts.CreatePersonId,
                    ConfirmPersonId = ts.ConfirmPersonId,
                    ApprovePersonId = ts.ApprovePersonId,
                    VerifyPersonId = ts.VerifyPersonId,
                    BlockIds = ts.TechnicalServiceBlocks.Select(tsb => tsb.BlockId).ToList(),
                    SystemIds = ts.TechnicalServiceSystems.Select(tss => tss.SystemId).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteTechnicalService(int technicalServiceId)
        {
            var technicalService = await _dbContext.TechnicalServices.FindAsync(technicalServiceId);

            if (technicalService == null)
            {
                return false; 
            }         
            _dbContext.TechnicalServiceBlocks.RemoveRange(technicalService.TechnicalServiceBlocks);
            _dbContext.TechnicalServiceSystems.RemoveRange(technicalService.TechnicalServiceSystems);
          
            _dbContext.TechnicalServices.Remove(technicalService);

            await _dbContext.SaveChangesAsync();

            return true; 
        }
        public async Task<IEnumerable<TechnicalServiceReportViewModel>> GetTechnicalServiceReport(
            string responsiblePersonName,
            List<string?> blockCodes,
            List<string?> systemCodes,
            DateTime? creationDate)
        {
            var technicalServicesCount = _dbContext.TechnicalServices.Count();
            Console.WriteLine($"Number of TechnicalServices in the database: {technicalServicesCount}");

            var query = _dbContext.TechnicalServices
                .Include(ts => ts.TechnicalServiceBlocks)
                .ThenInclude(tsb => tsb.Block)
                .Include(ts => ts.TechnicalServiceSystems)
                .ThenInclude(tss => tss.System)
                .AsQueryable();

            if (!string.IsNullOrEmpty(responsiblePersonName))
            {
                query = query
                    .Where(ts =>
                        ts.CreatePersonNames.Contains(responsiblePersonName) ||
                        ts.ConfirmPersonNames.Contains(responsiblePersonName) ||
                        ts.ApprovePersonNames.Contains(responsiblePersonName) ||
                        ts.VerifyPersonNames.Contains(responsiblePersonName));
            }

            if (blockCodes?.Any() == true)
            {
                query = query
                    .Where(ts => ts.TechnicalServiceBlocks.Any(tsb => blockCodes.Contains(tsb.Block.Code)));
            }

            if (systemCodes?.Any() == true)
            {
                query = query
                    .Where(ts => ts.TechnicalServiceSystems.Any(tss => systemCodes.Contains(tss.System.Code)));
            }

            if (creationDate.HasValue)
            {
                var startOfDay = creationDate.Value.Date;
                var endOfDay = startOfDay.AddDays(1);

                query = query.Where(ts => ts.CreationTime >= startOfDay && ts.CreationTime < endOfDay);
            }

            var result = await query
                .Select(ts => new TechnicalServiceReportViewModel
                {
                    Name = ts.Name,
                    Description = ts.Description,
                    Blocks = ts.TechnicalServiceBlocks.Select(tsb => tsb.Block.Code).ToList(),
                    Systems = ts.TechnicalServiceSystems.Select(tss => GetFullSystemName(tss.System)).ToList(),
                    ResponsiblePersons = new List<string>
                    {
                "creation = "+ ts.CreatePersonNames,
               "confirmation = "+  ts.ConfirmPersonNames,
               "approval = "+  ts.ApprovePersonNames,
               "verification = "+  ts.VerifyPersonNames
                    },
                    CreationTime = ts.CreationTime
                })
                .ToListAsync();

            return result;
        }
        private static string GetFullSystemName(TechnicalServiceTask.Data.System system)
        {
            if (system == null)
            {
                return string.Empty;
            }

            var fullName = new List<string> { system.Name };

           
            while (system.ParentSystem != null)
            {
                system = system.ParentSystem;
                fullName.Insert(0, system.Name); 
            }

            return string.Join(".", fullName);
        }
    }
}
