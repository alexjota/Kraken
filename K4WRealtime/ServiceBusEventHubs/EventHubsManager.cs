namespace ServiceBusEventHubs
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class EventHubsManager
    {
        private readonly string serviceBusConnectionString;
        private readonly string path;
        private readonly EventHubClient client;

        public EventHubsManager(string serviceBusConnectionString, string path)
        {
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.path = path;

            this.client = EventHubClient.CreateFromConnectionString(serviceBusConnectionString, path);
        }


        public async Task SendEventAsync(KinectEvent kinectEvent)
        {
            try
            {
                var serializedString = JsonConvert.SerializeObject(kinectEvent);

                var eventData = new EventData(Encoding.UTF8.GetBytes(serializedString))
                {
                    PartitionKey = kinectEvent.DeviceId.ToString()
                };

                eventData.Properties.Add("Type", "DepthData_" + DateTime.Now.ToLongTimeString());

                await this.client.SendAsync(eventData);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task StartReceiver(string storageConnectionString)
        {
            // state should be retrieved from blob storage.
            var consumerGroup = this.client.GetDefaultConsumerGroup();
            var eventProcessorHost = new EventProcessorHost("singleworker", this.path, consumerGroup.GroupName, this.serviceBusConnectionString, storageConnectionString);

            await eventProcessorHost.RegisterEventProcessorAsync<KinectDepthEventProcessor>();
        }
    }
}
