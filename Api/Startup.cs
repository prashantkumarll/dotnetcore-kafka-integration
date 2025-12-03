using Api.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
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
            // Register controllers (replaces AddMvc/CompatibilityVersion in old templates)
            services.AddControllers();

            // Bind legacy configs from configuration - use Get<Dictionary> for dot-notation support
            var producerConfigDict = Configuration.GetSection("producer").Get<Dictionary<string, string>>();
            var consumerConfigDict = Configuration.GetSection("consumer").Get<Dictionary<string, string>>();

            // Resolve Service Bus connection string from configuration (look in producer config or ServiceBus section)
            string connectionString = null;
            if (producerConfigDict != null && producerConfigDict.TryGetValue("connectionString", out var cs))
            {
                connectionString = cs;
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Configuration.GetValue<string>("ServiceBus:ConnectionString");
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Service Bus connection string not found in configuration.");
            }

            var serviceBusClient = new ServiceBusClient(connectionString);

            // Default processor options (can be extended from consumerConfigDict if needed)
            var processorOptions = new ServiceBusProcessorOptions();

            services.AddSingleton(serviceBusClient);
            services.AddSingleton(processorOptions);

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
}
