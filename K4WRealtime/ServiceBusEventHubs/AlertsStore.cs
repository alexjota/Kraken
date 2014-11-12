namespace ServiceBusEventHubs
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AlertsStore
    {
        private readonly string endpointUrl;
        private readonly string authKey;
        private readonly string databaseId;
        private const string CollectionId = "kinectalerts";
        private string collectionLink;
        
        private readonly ConnectionPolicy policy;
        private DocumentClient client;

        public AlertsStore(string endpointUrl, string authKey, string databaseId)
        {
            this.endpointUrl = endpointUrl;
            this.authKey = authKey;
            this.databaseId = databaseId;

            this.policy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };


            this.client = new DocumentClient(new Uri(this.endpointUrl), this.authKey, this.policy);
            var database = client.CreateDatabaseQuery().Where(db => db.Id == this.databaseId).ToArray().FirstOrDefault();
            DocumentCollection collection = this.client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id ==  CollectionId).ToArray().FirstOrDefault();
            
            this.collectionLink = collection.SelfLink;
        }

        public IEnumerable<KinectEvent> FindAll()
        {
            var alerts = from alert in this.client.CreateDocumentQuery<KinectEvent>(this.collectionLink)
                         select alert;

            return alerts;
        }

        public async Task Create(KinectEvent kinectEvent)
        {
            await this.client.CreateDocumentAsync(this.collectionLink, kinectEvent);
        }
    }
}
