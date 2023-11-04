using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Utilities
{
    public static class AgentUtilities
    {
        private const int MaxConcurrentChats = 10;
        private const int OverflowTeamCapacity = 6;
        private const double MaxQueueMultiplier = 1.5;

        public static List<Agent> AddAgent(List<Agent> agents, string name, string seniority)
        {
            var agent = new Agent
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                SeniorityLevel = seniority,
                Capacity = CalculateAgentCapacity(seniority),
                CurrentChats = 0
            };

            agents.Add(agent);
            return agents;
        }

        private static int CalculateAgentCapacity(string seniority)
        {
            int capacity = 0;

            switch (seniority)
            {
                case "Junior":
                    capacity = (int)(MaxConcurrentChats * 0.4);
                    break;
                case "MidLevel":
                    capacity = (int)(MaxConcurrentChats * 0.6);
                    break;
                case "Senior":
                    capacity = (int)(MaxConcurrentChats * 0.8);
                    break;
                case "TeamLead":
                    capacity = (int)(MaxConcurrentChats * 0.5);
                    break;
            }

            return capacity;
        }

        public static List<Agent> AddOverflowTeamMember(List<Agent> overflowTeam, string name)
        {
            var agent = new Agent
            {
                Id = new Guid().ToString(),
                Name = name,
                SeniorityLevel = "Junior",
                Capacity = MaxConcurrentChats,
                CurrentChats = 0
            };

            overflowTeam.Add(agent);
            return overflowTeam;
        }

    }


}
