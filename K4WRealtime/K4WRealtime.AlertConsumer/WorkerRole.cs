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
using Microsoft.ServiceBus.Messaging;

namespace K4WRealtime.AlertListener
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private QueueManager queueManager;

        public override void Run()
        {
            Trace.TraceInformation("K4WRealtime.AlertListener is running");

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
            var connectionString = CloudConfigurationManager.GetSetting("queueConnectionString");
            var path = CloudConfigurationManager.GetSetting("path");

            this.queueManager = new QueueManager(path, connectionString);
            this.queueManager.Start();


            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("K4WRealtime.AlertListener has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("K4WRealtime.AlertListener is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("K4WRealtime.AlertListener has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {

            this.queueManager.ReceiveMessages(this.ProcessMessage);

            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

        private async Task ProcessMessage(BrokeredMessage message)
        {
            await Task.Run(() => Trace.WriteLine("PROXIMITY ALERT!"));
        }
    }
}
