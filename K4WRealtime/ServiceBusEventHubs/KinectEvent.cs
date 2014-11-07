using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusEventHubs
{
    [DataContract]
    public class KinectEvent
    {
        [DataMember]
        public Guid DeviceId { get; set; }

        [DataMember]
        public int FrameNumber { get; set; }

        [DataMember]
        public int MinDepth { get; set; }

        [DataMember]
        public int MaxDepth { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}
