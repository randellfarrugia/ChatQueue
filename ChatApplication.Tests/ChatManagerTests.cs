using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ChatApplication.BusinessLogic;
using ChatApplication.Models;
using ChatApplication.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace ChatApplication.Tests.ChatManagerTests
{
    public class ChatManagerTests
    {
        [Fact]
        public void CreateNewSession_AddsSessionAndAssignsToAgent_WhenThereAreAvailableAgents()
        {
            // Arrange
            var chatManager = new Mock<ChatManager>();
            var userId = "user123";
            var agent = new Agent { Name = "TestAgent", SeniorityLevel = "Junior" };
            chatManager.Object.AddAgent(agent);

            // Act
            var result = chatManager.Object.CreateNewSession(userId);

            // Assert
            Assert.Equal("OK", result);
            Assert.Single(chatManager.Object.agents);
            Assert.Single(chatManager.Object.chatQueue);
            Assert.NotNull(chatManager.Object.chatQueue[0].AgentID);
        }

        [Fact]
        public void CreateNewSession_ReturnsNOTOK_WhenThereAreNoAvailableAgents()
        {
            // Arrange
            var chatManager = new Mock<ChatManager>();
            var userId = "user123";

            // Act
            var result = chatManager.Object.CreateNewSession(userId);

            // Assert
            Assert.Equal("NOTOK", result);
        }       

        [Fact]
        public void ResetPollStatus_ResetsPollCount()
        {
            // Arrange
            var chatManager = new Mock<ChatManager>();
            var userId = "user123";
            var agent = new Agent { Name = "TestAgent", SeniorityLevel = "Junior" };
            chatManager.Object.AddAgent(agent);
            chatManager.Object.CreateNewSession(userId);
            chatManager.Object.StartChat(userId);

            // Act
            chatManager.Object.ResetPollStatus(userId);

            // Assert
            var chatSession = chatManager.Object.chatQueue.First();
            Assert.Equal(0, chatSession.PollCount);
        }

        [Fact]
        public void AssignChatToAgent_AssignsChatToAvailableAgent()
        {
            // Arrange
            var chatManager = new Mock<ChatManager>();
            var userId = "user123";
            var agent = new Agent { Name = "TestAgent", SeniorityLevel = "Junior" };
            chatManager.Object.AddAgent(agent);
            var session = new ChatSession
            {
                SessionId = "12313-123123-13123123",
                UserId = userId,
                StartTime = DateTime.Now,
                IsActive = true,
                PollCount = 0
            };

            chatManager.Object.chatQueue.Add(session);

            // Act
            var result = chatManager.Object.AssignChatToAgent(chatManager.Object.chatQueue.First());

            // Assert
            Assert.Equal("OK", result);
            var chatSession = chatManager.Object.chatQueue.First();
            Assert.NotNull(chatSession.AgentID);
            Assert.Equal(1, chatManager.Object.agents.Where(a=> a.Id==chatSession.AgentID).FirstOrDefault().CurrentChats);
        }

        [Fact]
        public void AssignChatToAgent_ReturnsNOTOK_WhenNoAvailableAgents()
        {
            // Arrange
            var chatManager = new Mock<ChatManager>();
            var userId = "user123";

            // Act
            var result = chatManager.Object.AssignChatToAgent(new ChatSession());

            // Assert
            Assert.Equal("NOTOK", result);
        }
    }
}