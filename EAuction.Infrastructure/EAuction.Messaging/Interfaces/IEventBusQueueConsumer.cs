using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Messaging.Interfaces
{
    public interface IEventBusQueueConsumer :IEventConsumer
    {
        string QueueName { get; }
    }
}
