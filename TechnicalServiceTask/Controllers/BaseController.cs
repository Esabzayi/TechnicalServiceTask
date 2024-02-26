using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalServiceTask.Services;

namespace TechnicalServiceTask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly BaseService _baseService;

        public BaseController(BaseService baseService)
        {
            _baseService = baseService;
        }
    }
}
