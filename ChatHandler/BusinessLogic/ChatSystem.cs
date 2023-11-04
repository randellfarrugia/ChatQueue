using ChatApplication.Models;
using ChatApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ChatApplication.BusinessLogic
{
    public class ChatSystem
    {
        private const int MaxConcurrentChats = 10;
        private const int OverflowTeamCapacity = 6;
        private const double MaxQueueMultiplier = 1.5;

        private List<ChatSession> chatQueue;
        private List<Agent> agents;
        private List<Agent> overflowTeam;

        public ChatSystem()
        {
            chatQueue = new List<ChatSession>();
            agents = new List<Agent>();
            overflowTeam = new List<Agent>();
        }

        public string InsertNewSession(string userId)
        {
            Guid guid = Guid.NewGuid();
            var session = new ChatSession
            {
                SessionId = guid.ToString(),
                UserId = userId,
                StartTime = DateTime.Now,
                IsActive = true,
                PollCount = 0
            };

            var maxQueueLength = GetMaxQueueLength();
            var chatQueueCount = chatQueue?.Where(x => x.IsActive == true).ToList().Count;

            if (chatQueueCount < maxQueueLength)
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
            var session = chatQueue.Where(x => x.UserId == userId).FirstOrDefault();
            MonitorChatSession(session);
        }

        public void AddAgent(Agent agent)
        {
            agents = AgentUtilities.AddAgent(agents, agent.Name, agent.SeniorityLevel);
        }

        public void AddOverFlowAgent(Agent agent)
        {
            overflowTeam = AgentUtilities.AddOverflowTeamMember(overflowTeam, agent.Name);
        }

        public void UpdatePollStatus(string userId)
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



        private void PollChatSession(string sessionId)
        {
            var stopCheck = false;
            while (!stopCheck)
            {
                Thread.Sleep(4000);
                var pollCount = chatQueue?.Where(x => x.SessionId == sessionId).FirstOrDefault().PollCount;
                pollCount++;

                if (pollCount >= 3)
                {
                    chatQueue?.Where(x => x.SessionId == sessionId).ToList().ForEach(y => y.IsActive = false);
                    stopCheck = true;
                    agents.Where(a => a.Id == chatQueue?.Where(x => x.SessionId == sessionId).ToList()[0].AgentID).ToList().ForEach(x => x.CurrentChats--);
                }
                else
                {
                    chatQueue?.Where(x => x.SessionId == sessionId).ToList().ForEach(y => y.PollCount = pollCount);
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

        public int GetMaxQueueLength()
        {
            int capacity = agents.Sum(a => a.Capacity);
            int maxQueueLength = (int)(capacity * MaxQueueMultiplier);
            return maxQueueLength;
        }
    }
}
