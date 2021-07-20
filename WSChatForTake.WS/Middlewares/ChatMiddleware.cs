using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WSChatForTake.WS.Classes;
using WSChatForTake.WS.Collections;
using WSChatForTake.WS.Models;

namespace WSChatForTake.WS.Middlewares
{
    public class ChatMiddleware
    {

        private readonly RequestDelegate _next;

        public ChatMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var client = new ClientModel(await context.WebSockets.AcceptWebSocketAsync(), Guid.NewGuid().ToString(), string.Empty);
                    try
                    {
                        await Handle(client);
                    }
                    catch (Exception ex)
                    {
                        await context.Response.WriteAsync("closed");
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Handle(ClientModel client)
        {
            ClientCollection.Add(client);

            WebSocketReceiveResult result = null;
            do
            {
                var buffer = new byte[1024 * 4];
                result = await client.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType != WebSocketMessageType.Text || result.CloseStatus.HasValue) continue;
                var msgString = Encoding.UTF8.GetString(buffer);
                var message = JsonConvert.DeserializeObject<MessageModel>(msgString);
                message.SendToClientId = client.Id;
                MessageRoute(message);
            }
            while (!result.CloseStatus.HasValue);
            ClientCollection.Remove(client);
        }

        private void MessageRoute(MessageModel message)
        {
            var client = ClientCollection.Get(message.SendToClientId);
            switch (message.Action)
            {
                case "join":
                    client.Room = message.Room;
                    client.SendMessageAsync($"{message.Username} joined the room {client.Room} .");
                    break;
                case "send_to_room":
                    if (string.IsNullOrEmpty(client.Room))
                    {
                        break;
                    }
                    var clients = ClientCollection.GetRoomClients(client.Room);
                    clients.ForEach(c =>
                    {
                        c.SendMessageAsync(message.Username + " : " + message.Message);
                    });
                    break;
                case "leave":
                    var room = client.Room;
                    client.Room = "";
                    client.SendMessageAsync($"{message.Username} leaved the room {room} .");
                    break;
                default:
                    break;
            }
        }
    }
}
