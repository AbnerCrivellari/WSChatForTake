using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSChatForTake.WS.Classes;

namespace WSChatForTake.WS.Collections
{
    public class ClientCollection
    {
        private static List<ClientModel> _clientsList = new();

        public static void Add(ClientModel client)
        {
            _clientsList.Add(client);
        }

        public static void Remove(ClientModel client)
        {
            _clientsList.Remove(client);
        }

        public static ClientModel Get(string clientId)
        {
            var client = _clientsList.FirstOrDefault(c=>c.Id == clientId);

            return client;
        }

        public static List<ClientModel> GetRoomClients(string room)
        {
            var client = _clientsList.Where(c => c.Room == room);
            return client.ToList();
        }
    }
}
