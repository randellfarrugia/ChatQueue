using Xunit;
using System;
using System.Collections.Generic;
using ChatApplication.Utilities;
using ChatApplication.Models;

namespace ChatApplication.Tests.AgentUtilities
{
    public class AgentUtilitiesTests
    {
        [Fact]
        public void AddAgent_AddsAgentToList()
        {
            // Arrange
            var agents = new List<Agent>();
            var name = "TestAgent";
            var seniority = "Junior";
            var expectedCapacity = (int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.4);

            // Act
            var result = Utilities.AgentUtilities.AddAgent(agents, name, seniority);

            // Assert
            Assert.Single(result);
            var addedAgent = result[0];
            Assert.Equal(name, addedAgent.Name);
            Assert.Equal(seniority, addedAgent.SeniorityLevel);
            Assert.Equal(expectedCapacity, addedAgent.Capacity);
            Assert.Equal(0, addedAgent.CurrentChats);
        }

        [Fact]
        public void AddOverflowTeamMember_AddsAgentToOverflowTeam()
        {
            // Arrange
            var overflowTeam = new List<Agent>();
            var name = "TestOverflowAgent";

            // Act
            var result = Utilities.AgentUtilities.AddOverflowTeamMember(overflowTeam, name);

            // Assert
            Assert.Single(result);
            var addedAgent = result[0];
            Assert.Equal(name, addedAgent.Name);
            Assert.Equal("Junior", addedAgent.SeniorityLevel);
            Assert.Equal((int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.4), addedAgent.Capacity);
            Assert.Equal(0, addedAgent.CurrentChats);
        }

        [Theory]
        [InlineData("Junior", (int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.4))]
        [InlineData("MidLevel", (int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.6))]
        [InlineData("Senior", (int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.8))]
        [InlineData("TeamLead", (int)(Utilities.AgentUtilities.MaxConcurrentChats * 0.5))]
        public void CalculateAgentCapacity_ReturnsCorrectCapacity(string seniority, int expectedCapacity)
        {
            // Act
            int capacity = Utilities.AgentUtilities.CalculateAgentCapacity(seniority);

            // Assert
            Assert.Equal(expectedCapacity, capacity);
        }
    }
}