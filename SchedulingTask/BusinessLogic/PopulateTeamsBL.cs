using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using QueueAPI.Controllers;
using Serilog;
using ILogger = Serilog.ILogger;
using Microsoft.Extensions.Hosting;
using ChatApplication.BusinessLogic;
using ChatApplication.Models;


namespace QueueAPI.BusinessLogic
{
    public class PopulateTeamsBL : IQueueBL
    {
        public ILogger log;
        public ChatSystem chatHandler;

        public PopulateTeamsBL(IConfiguration configuration, ILogger _logger, ChatSystem chatSystem)
        {
            log = _logger;
            chatHandler = chatSystem;
        }

        public void PopulateTeamA()
        {
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "Junior" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "MidLevel" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "MidLevel" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "TeamLead" });
        }
        public void PopulateTeamB()
        {
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "Junior" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "Junior" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "MidLevel" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "Senior" });
        }
        public void PopulateTeamC()
        {
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "MidLevel" });
            chatHandler.AddAgent(new Agent() { SeniorityLevel = "MidLevel" });
        }
        public void PopulateOverflowTeam()
        {
            for (int i = 0; i < 6; i++)
            {
                chatHandler.AddOverFlowAgent(new Agent());
            }            
        }

    }
}
