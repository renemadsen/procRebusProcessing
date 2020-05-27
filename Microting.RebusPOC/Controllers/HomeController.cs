using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microting.RebusPOC.Service.Managers;
using System.Threading.Tasks;

namespace Microting.RebusPOC.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMasterManager _manager;

        public HomeController(ILogger<HomeController> logger, IMasterManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet]
        [Route("trigger")]
        public async Task<IActionResult> Trigger(long id)
        {
            await _manager.Trigger(id);

            return Ok();
        }
    }
}
