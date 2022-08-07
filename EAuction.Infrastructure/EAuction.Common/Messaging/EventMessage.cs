using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Common.Messaging
{
    public class EventMessage
    {
        public string SessionId { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
    }
}
