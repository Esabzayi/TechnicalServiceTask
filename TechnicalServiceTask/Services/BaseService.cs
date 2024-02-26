using TechnicalServiceTask.Data;

namespace TechnicalServiceTask.Services
{
    public class BaseService
    {
        protected readonly AppEntity _dbContext;

        public BaseService(AppEntity dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
