using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
using QueueAPI.BusinessLogic;
using ChatApplication.BusinessLogic;

namespace QueueAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PopulateTeamsController : ControllerBase
    {
        public PopulateTeamsBL PopulateTeamsBL;
        public ILogger log;
        IConfiguration configuration;

        public PopulateTeamsController(IConfiguration _configuration, ILogger _logger, ChatManager chatSystem)
        {
            PopulateTeamsBL = new PopulateTeamsBL(_configuration, _logger, chatSystem);
            configuration = _configuration;
            log = _logger;
        }

        [HttpPost]
        public IActionResult AddTeamA()
        {
            PopulateTeamsBL.PopulateTeamA();
            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost]
        public IActionResult AddTeamB()
        {
            PopulateTeamsBL.PopulateTeamB();
            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost]
        public IActionResult AddTeamC()
        {
            PopulateTeamsBL.PopulateTeamC();
            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost]
        public IActionResult AddOverFlowTeam()
        {
            PopulateTeamsBL.PopulateOverflowTeam();
            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };
        }

    }
}
