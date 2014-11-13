using Microsoft.AspNet.SignalR.Client;
using ServiceBusEventHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K4WRealtime.ServiceBusEventHubs
{
    public class HubClient
    {
        private readonly string hubUrl;
        private IHubProxy proxy;

        public HubClient(string hubUrl)
        {
            this.hubUrl = hubUrl;
        }

        public async Task StartAsync()
        {
            var connection = new HubConnection(this.hubUrl);
            this.proxy = connection.CreateHubProxy("AlertsHub");
           
            await connection.Start();
        }

        public async Task SendUpdateAsync(KinectEvent kinectEvent)
        {
            await this.proxy.Invoke("SendUpdate", kinectEvent);
        }
    }
}
