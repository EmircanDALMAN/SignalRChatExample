using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRChatExample.ServerUI.Models;

namespace SignalRChatExample.ServerUI.Data
{
    public static class ClientSource
    {
        public static List<Client> Clients { get; } = new List<Client>();
    }
}
