using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Messaging.Interfaces
{
    public interface IEventConsumer
    {
        Task StartProcessingAsync();

        event Func<string, Task> Consume;
    }
}
