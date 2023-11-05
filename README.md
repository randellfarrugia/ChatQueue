# ChatQueue

This solution contains two projects. 

- QueueAPI
- ChatApplication

### QueueAPI

This project acts as an API endpoint which contains methods to populate agent data for all the different teams and also provide a way to initiate the chat session.
Included with this project there is a swagger documentation which will allow you to test out the different endpoints

### Chat Application

This project is a class library which handles all the logic in regards to the addition of data, initiation of requests and monitoring of chat sessions.

For the monitoring of sessions, the client is polling every 1 second to reset the poll count to 0, meanwhile the Chat Manager checks the client's poll count every 4 seconds. 
If the client's poll count reaches 3 this means that the client hasn't updated his poll count and has exited the chat session. The chat session will be marked as inactive and the Agent will be unassigned this chat.
