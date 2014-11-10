using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace K4WRealtime.Web.Hubs
{
    [HubName("alertsHub")]
    public class AlertsHub : Hub
    {
        public void SendUpdate(string message)
        {
            Clients.All.alertReceived(message);
        }
    }
}