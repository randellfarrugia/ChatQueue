using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
using QueueAPI.BusinessLogic;
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

        public QueueController(IConfiguration _configuration, ILogger _logger, IChatManager chatManager)
        {
            QueueBL = new QueueBL(_configuration, _logger, chatManager);
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
