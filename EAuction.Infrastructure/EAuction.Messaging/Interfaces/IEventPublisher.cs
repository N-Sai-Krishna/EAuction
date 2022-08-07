using EAuction.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Messaging.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishMessageAsync(EventMessage messageToPublish);
    }
}
