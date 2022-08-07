using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Messaging.Interfaces
{
    public interface IEventBusTopicPublisher : IEventPublisher
    {
        string TopicName { get; }
    }
}
