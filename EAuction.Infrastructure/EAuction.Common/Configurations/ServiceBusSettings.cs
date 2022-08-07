using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Common.Configurations
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }

        public IEnumerable<string> ServiceBusQueues { get; set; }
        public IEnumerable<string> ServiceBusTopics { get; set; }
        public IEnumerable<string> ServiceBusSubscribers { get; set; }
        public IEnumerable<string> ServiceBusQueueConsumers { get; set; }

    }
}
