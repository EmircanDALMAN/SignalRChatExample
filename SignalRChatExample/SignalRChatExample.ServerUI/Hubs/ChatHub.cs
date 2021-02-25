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
            await Clients.Others.SendAsync("clientJoined", nickName);
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }

        public async Task SendMessageAsync(string message, string clientName)
        {
            clientName = clientName.Trim();
            var senderClient = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (clientName == "Tümü")
            {
                if (senderClient != null)
                    await Clients.Others.SendAsync("receiveMessage", message, senderClient.NickName);
            }
            else
            {
                var client = ClientSource.Clients.FirstOrDefault(c => c.NickName == clientName);
                if (client != null && senderClient != null)
                {
                    await Clients.Client(client.ConnectionId)
                        .SendAsync("receiveMessage", message, senderClient.NickName);
                }
            }

        }

        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            Group group = new Group { GroupName = groupName };
            group.Clients.Add(ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId));

            GroupSource.Groups.Add(group);

            await Clients.All.SendAsync("groups", GroupSource.Groups);
        }

        public async Task AddClientToGroup(IEnumerable<string> groupNames)
        {
            var client = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            foreach (var groupName in groupNames)
            {
                var group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);
                var result = group.Clients.Any(c => c.ConnectionId == Context.ConnectionId);
                if (!result)
                {
                    group.Clients.Add(client);
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                }
            }
        }

        public async Task GetClientToGroup(string groupName)
        {
            var group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);
            await Clients.Caller.SendAsync("clients",
                groupName == "-1" ? ClientSource.Clients : group.Clients);
        }

        public async Task SendMessageToGroupAsync(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("receiveMessage", message,
                ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId)?.NickName);
        }
    }
}
