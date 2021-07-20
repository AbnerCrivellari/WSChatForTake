using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSChatForTake.WS.Models
{
    public class MessageModel
    {
        public string SendToClientId { get; set; }

        public string Action { get; set; }

        public string Room { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }
    }
}
