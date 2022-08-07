using System;
using System.Collections.Generic;
using System.Text;

namespace EAuction.Core.Common.Exceptions
{
    public class EAuctionDomainException : Exception
    {
        public EAuctionDomainException(string message) : base(message)
        {

        }
    }
}
