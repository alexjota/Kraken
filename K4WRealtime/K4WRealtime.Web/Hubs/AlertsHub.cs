using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace K4WRealtime.Web.Hubs
{
    public class AlertsHub : Hub
    {
        public void AlertReceived()
        {
            Clients.All.alertReceived();
        }
    }
}