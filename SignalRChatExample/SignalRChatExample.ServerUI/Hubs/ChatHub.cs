using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRChatExample.ServerUI.Data;
using SignalRChatExample.ServerUI.Models;

namespace SignalRChatExample.ServerUI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task GetNickName(string nickName)
        {
            Client client = new Client
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickName
            };
            ClientSource.Clients.Add(client);
            await Clients.Others.SendAsync("clientJoined",nickName);
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }
    }
}
