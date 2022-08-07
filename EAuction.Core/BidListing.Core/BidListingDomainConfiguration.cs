using BidListing.Core.Consumer;
using BidListing.Core.Domain;
using BidListing.Core.Repositories;
using BidListing.Core.Services;
using EAuction.Common.Messaging;
using EAuction.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BidListing.Core
{
    public static class BidListingDomainConfiguration
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ProductAndBidDetails, string>, ProductAndBidDetailsRepository>();
            services.AddScoped<IBidListingService, BidListingService>();
            services.AddSingleton<IConsumerHandler, AddProductConsumer>();
            services.AddSingleton<IConsumerHandler, BidAddedOrUpdatedConsumer>();
        }

        public static void InitializeConsumers(this IApplicationBuilder app)
        {
            foreach (var consumer in app.ApplicationServices.GetServices<IConsumerHandler>())
            {
                consumer.RegisterAsync().Wait();
            }
        }
    }
}
