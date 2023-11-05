using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;
using ChatApplication.BusinessLogic;
using ChatApplication.Models;


namespace QueueAPI.BusinessLogic
{
    public class QueueBL
    {
        public ILogger log;
        public ChatManager chatHandler;
        IHttpContextAccessor context;

        public QueueBL(IConfiguration configuration, IHttpContextAccessor _context, ILogger _logger, IChatManager chatManager)
        {
            log = _logger;
            chatHandler = (ChatManager?)chatManager;
            context = _context;
        }

        public IActionResult InsertNewSession(QueueRequest request)
        {
            log.Information($"Request : {JsonConvert.SerializeObject(request)}");
            string result = chatHandler.CreateNewSession(request.UserId);

            if (result == "NOTOK")
            {
                return new ContentResult() { Content = "{\"Result\":\"NOTOK\"}", ContentType = "application/json", StatusCode = 400 };
            }

            chatHandler.StartChat(request.UserId);
            PollingMethod(request.UserId);

            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };

        }
        public void PollingMethod(string userId)
        {
            bool? RequestAborted = false;
            while ((bool)!RequestAborted)
            {
                RequestAborted = context?.HttpContext?.RequestAborted.IsCancellationRequested;
                if ((RequestAborted == null) || (RequestAborted == true))
                {
                    RequestAborted = true;
                }
                else
                {
                    Thread.Sleep(10000);
                    chatHandler.ResetPollStatus(userId);
                }
            }

        }
    }
}
