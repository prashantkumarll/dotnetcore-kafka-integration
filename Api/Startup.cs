using Api.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Registers services into DI
        public void ConfigureServices(IServiceCollection services)
        {
            // Register controllers
            services.AddControllers();

            // Configure Service Bus connection
            var serviceBusConnectionString = Configuration["ServiceBus:ConnectionString"];
            var queueName = Configuration["ServiceBus:QueueName"];

            // Register ServiceBusClient as singleton
            services.AddSingleton(sp => 
                new ServiceBusClient(serviceBusConnectionString));

            // Register ServiceBusProcessor or ServiceBusReceiver as scoped/singleton
            services.AddSingleton(sp => 
            {
                var client = sp.GetRequiredService<ServiceBusClient>();
                return client.CreateProcessor(queueName);
            });

            // Register the hosted/background service
            services.AddHostedService<ProcessOrdersService>();
        }

        // Configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // HSTS for non-development environments
                app.UseHsts();
            }

            // Uncomment if you want automatic redirect to HTTPS
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}