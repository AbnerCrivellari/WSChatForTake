using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSChatForTake.WS.Classes
{
    public class ClientModel
    {
        public WebSocket WebSocket { get; }

        public string Id { get; }

        public string Room { get; set; }

        public Task SendMessageAsync(string message)
        {
            var msg = Encoding.UTF8.GetBytes(message);
            return WebSocket.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public ClientModel(WebSocket webSocket, string id, string room)
        {
            WebSocket = webSocket;
            Id = id;
            Room = room;
        }
    }
}
