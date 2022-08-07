using Buyer.Core;
using EAuction.Common.Configurations;
using EAuction.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Buyer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.Configure<DatabaseSettings>(Configuration.GetSection("BuyerDatabase"));
            services.Configure<ServiceBusSettings>(Configuration.GetSection("ServiceBusSettings"));

            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<MongoClient>((_) =>
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_.GetService<IOptions<DatabaseSettings>>().Value.ConnectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                return new MongoClient(settings);
            });

            services.AddSingleton<IMongoDatabase>((_) => _.GetService<MongoClient>().GetDatabase(_.GetService<IOptions<DatabaseSettings>>().Value.Store));

            services.ConfigureMessaging(Configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>());

            services.ConfigureDomainServices();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyerAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyerAPI v1"));
            }
            app.UsePathBase(new PathString("/e-Auction"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
