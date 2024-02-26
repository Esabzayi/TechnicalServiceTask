using Microsoft.EntityFrameworkCore;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Exceptions;
using TechnicalServiceTask.Models;

namespace TechnicalServiceTask.Services
{
    public class SystemService : BaseService
    {
        public SystemService(AppEntity dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<SystemViewModel>> GetSystemsViewModels()
        {
            var systems = await _dbContext.Systems.ToListAsync();
            return systems.Select(s => new SystemViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                ParentSystemId = s.ParentSystemId
            });
        }

        public async Task<SystemViewModel> GetSystemViewModelById(int id)
        {
            var system = await _dbContext.Systems.FindAsync(id);

            if (system == null)
                return null;

            return new SystemViewModel
            {
                Id = system.Id,
                Name = system.Name,
                Code = system.Code,
                ParentSystemId = system.ParentSystemId
            };
        }

        public async Task<SystemViewModel> CreateSystem(SystemViewModel systemViewModel)
        {
            var systemEntity = new TechnicalServiceTask.Data.System
            {
                Name = systemViewModel.Name,
                Code = systemViewModel.Code,
                ParentSystemId = systemViewModel.ParentSystemId
            };

            _dbContext.Systems.Add(systemEntity);
            await _dbContext.SaveChangesAsync();

            return new SystemViewModel
            {
                Id = systemEntity.Id,
                Name = systemEntity.Name,
                Code = systemEntity.Code,
                ParentSystemId = systemEntity.ParentSystemId
            };
        }

        public async Task UpdateSystem(int id, SystemViewModel systemViewModel)
        {
            var systemEntity = await _dbContext.Systems.FindAsync(id);

            if (systemEntity == null)
                throw new NotFoundException("System not found");

            systemEntity.Name = systemViewModel.Name;
            systemEntity.Code = systemViewModel.Code;
            systemEntity.ParentSystemId = systemViewModel.ParentSystemId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSystem(int id)
        {
            var systemEntity = await _dbContext.Systems.FindAsync(id);

            if (systemEntity == null)
                throw new NotFoundException("System not found");

            _dbContext.Systems.Remove(systemEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
