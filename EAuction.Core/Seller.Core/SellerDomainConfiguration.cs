using EAuction.Common.Messaging;
using EAuction.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Seller.Core.Consumer;
using Seller.Core.Domain;
using Seller.Core.Repositories;
using Seller.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Core
{
    public static class SellerDomainConfiguration
    {

        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IRepository<AuctionProduct, string>, ProductRepository>();
            services.AddScoped<IRepository<AuctionProductSeller, string>, SellerRepository>();
            services.AddSingleton<IConsumerHandler, DeleteProductConfirmSubscriber>();
            services.AddSingleton<IConsumerHandler, ValidateBidRequestSubscriber>();
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
