namespace ChatApplication.Models
{
    public class Agent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SeniorityLevel { get; set; }
        public int Capacity { get; set; }
        public int CurrentChats { get; set; }
    }
}
