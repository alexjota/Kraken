using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ServiceBusEventHubs;

namespace K4WRealtime.Web.Hubs
{
    [HubName("alertsHub")]
    public class AlertsHub : Hub
    {
        public void SendUpdate(KinectEvent kinectEvent)
        {
            Clients.All.alertReceived(kinectEvent);
        }

        public void SendOnlineNotification(string deviceId)
        {
            Clients.All.onlineNotificationReceived(deviceId);
        }

        public void SendOfflineNotification(string deviceId)
        {
            Clients.All.offlineNotificationReceived(deviceId);
        }
    }
}