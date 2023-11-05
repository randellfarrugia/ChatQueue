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

        public QueueBL(IConfiguration configuration, ILogger _logger, IChatManager chatManager)
        {
            log = _logger;
            chatHandler = (ChatManager?)chatManager;
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

            Thread poll = new Thread(
            o =>
               {
                   PollingMethod((string)o);
               });
            poll.Start(request.UserId);

            return new ContentResult() { Content = "{\"Result\":\"OK\"}", ContentType = "application/json", StatusCode = 200 };

        }
        public void PollingMethod(string userId)
        {
            while (true)
            {
                Thread.Sleep(10000);
                chatHandler.ResetPollStatus(userId);
            }

        }
    }
}
