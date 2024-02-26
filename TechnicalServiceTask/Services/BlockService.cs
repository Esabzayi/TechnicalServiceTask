using Microsoft.EntityFrameworkCore;
using TechnicalServiceTask.Data;
using TechnicalServiceTask.Exceptions;
using TechnicalServiceTask.Models;

namespace TechnicalServiceTask.Services
{
    public class BlockService : BaseService
    {
        public BlockService(AppEntity dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BlockViewModel>> GetBlockViewModels()
        {
           
            var blocks = await _dbContext.Blocks.ToListAsync();
            return blocks.Select(b => new BlockViewModel {Id = b.Id, Name = b.Name, Code = b.Code });
        }

        public async Task<BlockViewModel> GetBlockViewModelById(int id)
        {
           
            var block = await _dbContext.Blocks.FindAsync(id);

            if (block == null)
                return null;

            return new BlockViewModel { Name = block.Name, Code = block.Code };
        }

        public async Task<BlockViewModel> CreateBlock(BlockViewModel blockViewModel)
        {
           
            var blockEntity = new Block { Name = blockViewModel.Name, Code = blockViewModel.Code };
            _dbContext.Blocks.Add(blockEntity);
            await _dbContext.SaveChangesAsync();

           
            return new BlockViewModel {Id = blockEntity.Id, Name = blockEntity.Name, Code = blockEntity.Code };
        }

        public async Task UpdateBlock(int id, BlockViewModel blockViewModel)
        {
            
            var blockEntity = await _dbContext.Blocks.FindAsync(id);

            if (blockEntity == null)
                throw new NotFoundException("Block not found");

            blockEntity.Name = blockViewModel.Name;
            blockEntity.Code = blockViewModel.Code;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBlock(int id)
        {
          
            var blockEntity = await _dbContext.Blocks.FindAsync(id);

            if (blockEntity == null)
                throw new NotFoundException("Block not found");

            _dbContext.Blocks.Remove(blockEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
