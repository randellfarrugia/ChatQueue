using ChatApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.BusinessLogic
{
    public  interface IChatManager
    {
        public string CreateNewSession(string userId);
        public void StartChat(string userId);
        public void AddAgent(Agent agent);
        public void AddOverflowAgent(Agent agent);
        public void ResetPollStatus(string userId);
    }
}
