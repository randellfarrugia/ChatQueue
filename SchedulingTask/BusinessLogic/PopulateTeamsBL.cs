using ILogger = Serilog.ILogger;
using ChatApplication.BusinessLogic;
using ChatApplication.Models;


namespace QueueAPI.BusinessLogic
{
    public class PopulateTeamsBL
    {
        public ILogger log;
        public ChatManager chatHandler;

        public PopulateTeamsBL(IConfiguration configuration, ILogger _logger, ChatManager chatManager)
        {
            log = _logger;
            chatHandler = chatManager;
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
                chatHandler.AddOverflowAgent(new Agent());
            }            
        }

    }
}
