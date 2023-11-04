using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using QueueAPI.Controllers;
using Serilog;
using ILogger = Serilog.ILogger;
using Microsoft.Extensions.Hosting;
using ChatApplication.BusinessLogic;
using ChatApplication.Models;
using System.Threading;


namespace QueueAPI.BusinessLogic
{
    public class QueueBL : IQueueBL
    {
        public ILogger log;
        public ChatSystem chatHandler;

        public QueueBL(IConfiguration configuration, ILogger _logger, ChatSystem chatSystem)
        {
            log = _logger;
            chatHandler = chatSystem;
        }

        public IActionResult InsertNewSession(QueueRequest request)
        {
            log.Information($"Request : {JsonConvert.SerializeObject(request)}");
            string result = chatHandler.InsertNewSession(request.UserId);

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
                Thread.Sleep(1000);
                chatHandler.UpdatePollStatus(userId);
            }

        }
    }
}
