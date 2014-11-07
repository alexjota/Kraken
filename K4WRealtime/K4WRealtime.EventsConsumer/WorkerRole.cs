using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using ServiceBusEventHubs;

namespace KinectEventsConsumer
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private  ServiceBusEventHubs.EventHubsManager eventHubsManager;

        public override void Run()
        {
            Trace.TraceInformation("KinectEventsConsumer is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            var serviceBusConnectionString = CloudConfigurationManager.GetSetting("serviceBusConnectionString");
            var path = CloudConfigurationManager.GetSetting("path");

            this.eventHubsManager = new EventHubsManager(serviceBusConnectionString, path);


            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("KinectEventsConsumer has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("KinectEventsConsumer is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("KinectEventsConsumer has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var storageConnectionString = CloudConfigurationManager.GetSetting("storageConnectionString");
            await this.eventHubsManager.StartReceiver(storageConnectionString);

            //// TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                //Trace.TraceInformation("Working");
                
            }
        }
    }
}
