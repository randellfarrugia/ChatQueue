using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Serilog;
using ILogger = Serilog.ILogger;
using QueueAPI.BusinessLogic;
using ChatApplication.Models;
using ChatApplication.BusinessLogic;

namespace QueueAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        public QueueBL QueueBL;
        public ILogger log;
        IConfiguration configuration;

        public QueueController(IConfiguration _configuration, IHttpContextAccessor _context, ILogger _logger, ChatSystem chatSystem)
        {
            QueueBL = new QueueBL(_configuration, _logger, chatSystem);
            configuration = _configuration;
            log = _logger;
        }
        
        [HttpPost]
        public IActionResult CreateQueueSession([FromBody] ChatApplication.Models.QueueRequest value)
        {
            var result = QueueBL.InsertNewSession(value);
            return result;
        }

    }
}
