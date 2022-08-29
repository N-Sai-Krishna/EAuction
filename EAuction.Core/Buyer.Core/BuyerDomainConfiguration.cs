using Buyer.Core.Consumers;
using Buyer.Core.Domain;
using Buyer.Core.Repositories;
using Buyer.Core.Services;
using EAuction.Common.Messaging;
using EAuction.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buyer.Core
{
    public static class BuyerDomainConfiguration
    {
        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IRepository<AuctionBuyer, string>, BuyerRepository>();
            services.AddScoped<IBuyerService, BuyerService>();
            services.AddSingleton<IConsumerHandler, ProductDeleteRequestConsumer>();
            services.AddSingleton<IConsumerHandler, AddOrUpdateBidConsumer>();
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
