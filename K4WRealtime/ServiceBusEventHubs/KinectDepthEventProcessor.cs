﻿namespace ServiceBusEventHubs
{
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class KinectDepthEventProcessor : IEventProcessor
    {
        private PartitionContext partitionContext;
        private Stopwatch checkpointStopWatch;
        private QueueManager queueManager;

        public KinectDepthEventProcessor()
        {
            var queueConnectionString = CloudConfigurationManager.GetSetting("queueConnectionString");
            var queueName = CloudConfigurationManager.GetSetting("queuePath");
            this.queueManager = new QueueManager(queueName, queueConnectionString);
            this.queueManager.Start();
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine(string.Format("KinectDepthEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
            this.partitionContext = context;
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();

            return Task.FromResult<object>(null); 
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
                foreach (var message in messages)
                {
                    var kinectEvent = JsonConvert.DeserializeObject<KinectEvent>(Encoding.UTF8.GetString(message.GetBytes()));

                    //Trace.WriteLine(string.Format("Message received.  Partition: '{0}', Device: '{1}', MinDepth: '{2}'", this.partitionContext.Lease.PartitionId, kinectEvent.DeviceId, kinectEvent.MinDepth)); 

                    if (kinectEvent.MinDepth > 0 && kinectEvent.MinDepth < 500)
                    {
                        // flag alert send to servicebus queue
                        await this.queueManager.SendMessageAsync(kinectEvent);
                    }
                }

                //Call checkpoint every 1 minute1, so that worker can resume processing from the 1 minutes back if it restarts. 
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromSeconds(10))
                {
                    await context.CheckpointAsync();
                    lock (this)
                    {
                        this.checkpointStopWatch.Restart();
                    }
                } 
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine(string.Format("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }
    }
}
