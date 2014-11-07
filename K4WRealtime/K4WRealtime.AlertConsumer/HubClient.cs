using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K4WRealtime.AlertListener
{
    public class HubClient
    {
        private readonly string hubUrl;

        public HubClient(string hubUrl)
        {
            this.hubUrl = hubUrl;
        }

        public async Task StartAsync()
        {
            var connection = new HubConnection(this.hubUrl);
            var proxy = connection.CreateHubProxy("AlertsHub");

            await connection.Start();
        }
    }
}
