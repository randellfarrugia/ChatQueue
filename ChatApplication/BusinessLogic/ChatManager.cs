using ChatApplication.Models;
using ChatApplication.Utilities;

namespace ChatApplication.BusinessLogic
{
    public class ChatManager : IChatManager
    {
        private const int MaxConcurrentChats = 10;
        private const double MaxQueueMultiplier = 1.5;

        public List<ChatSession> chatQueue;
        public List<Agent> agents;
        public List<Agent> overflowTeam;

        public ChatManager()
        {
            chatQueue = new List<ChatSession>();
            agents = new List<Agent>();
            overflowTeam = new List<Agent>();
        }

        public string CreateNewSession(string userId)
        {
            Guid sessionId = Guid.NewGuid();
            var session = new ChatSession
            {
                SessionId = sessionId.ToString(),
                UserId = userId,
                StartTime = DateTime.Now,
                IsActive = true,
                PollCount = 0
            };

            var maxQueueLength = CalculateMaxQueueLength();
            var activeChatCount = chatQueue?.Count(x => x.IsActive);

            if (activeChatCount < maxQueueLength)
            {
                chatQueue.Add(session);
                return AssignChatToAgent(session);
            }
            else
            {
                return "NOTOK";
            }
        }

        public void StartChat(string userId)
        {
            var session = chatQueue.FirstOrDefault(x => x.UserId == userId);
            MonitorChatSession(session);
        }

        public void AddAgent(Agent agent)
        {
            agents = AgentUtilities.AddAgent(agents, agent.Name, agent.SeniorityLevel);
        }

        public void AddOverflowAgent(Agent agent)
        {
            overflowTeam = AgentUtilities.AddOverflowTeamMember(overflowTeam, agent.Name);
        }

        public void ResetPollStatus(string userId)
        {
            chatQueue.Where(x => x.UserId == userId).ToList().ForEach(y => y.PollCount = 0);
        }

        private void MonitorChatSession(ChatSession session)
        {
            Thread poll = new Thread(
            o =>
            {
                PollChatSession((string)o);
            });
            poll.Start(session.SessionId);
        }

        private void PollChatSession(object sessionId)
        {
            var stopCheck = false;
            while (!stopCheck)
            {
                Thread.Sleep(4000);
                var session = chatQueue?.FirstOrDefault(x => x.SessionId == (string)sessionId);
                if (session != null)
                {
                    session.PollCount++;
                    if (session.PollCount >= 3)
                    {
                        session.IsActive = false;
                        stopCheck = true;
                        var agentId = chatQueue.FirstOrDefault(x => x.SessionId == session.SessionId)?.AgentID;
                        if (agentId != null)
                        {
                            agents.Where(a => a.Id == agentId).ToList().ForEach(x => x.CurrentChats--);
                        }
                    }
                }
                else
                {
                    stopCheck = true;
                }
            }
        }

        public string AssignChatToAgent(ChatSession session)
        {
            var availableAgents = agents.Where(a => a.CurrentChats < a.Capacity).ToList();
            var availableOverflowAgents = overflowTeam.Where(a => a.CurrentChats < MaxConcurrentChats).ToList();

            if (availableAgents.Count == 0 && availableOverflowAgents.Count == 0)
            {
                return "NOTOK";
            }

            var agentToAssign = GetNextAvailableAgent(availableAgents, availableOverflowAgents);
            if (agentToAssign != null)
            {
                agentToAssign.CurrentChats++;
                chatQueue.Where(c => c.UserId == session.UserId).ToList().ForEach(x => x.AgentID = agentToAssign.Id);
                return "OK";
            }

            return "NOTOK";
        }

        private Agent GetNextAvailableAgent(List<Agent> availableAgents, List<Agent> availableOverflowAgents)
        {
            var juniorAgents = availableAgents.Where(a => a.SeniorityLevel == "Junior").ToList();
            var midLevelAgents = availableAgents.Where(a => a.SeniorityLevel == "MidLevel").ToList();
            var seniorAgents = availableAgents.Where(a => a.SeniorityLevel == "Senior").ToList();
            var teamLeadAgents = availableAgents.Where(a => a.SeniorityLevel == "TeamLead").ToList();

            var agentsBySeniority = new List<List<Agent>> { juniorAgents, midLevelAgents, seniorAgents, teamLeadAgents };

            foreach (var agentList in agentsBySeniority)
            {
                if (agentList.Count > 0)
                {
                    return agentList.First();
                }
            }

            if (availableOverflowAgents.Count > 0)
            {
                return availableOverflowAgents.First();
            }

            return null;
        }

        public int CalculateMaxQueueLength()
        {
            int totalCapacity = agents.Sum(a => a.Capacity);
            int maxQueueLength = (int)(totalCapacity * MaxQueueMultiplier);
            return maxQueueLength;
        }
    }
}
