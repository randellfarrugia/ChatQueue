namespace ChatApplication.Models
{
    public class ChatSession
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
        public int? PollCount { get; set; }
        public string AgentID { get; set; }
    }
}
