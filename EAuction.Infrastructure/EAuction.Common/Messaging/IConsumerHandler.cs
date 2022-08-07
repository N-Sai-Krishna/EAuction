using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Common.Messaging
{
    public interface IConsumerHandler
    {
        public Task RegisterAsync();

        public Task HandleMessageAsync(string message);

    }
}
