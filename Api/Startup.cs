using Api.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
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

            // Bind Service Bus configs from configuration - use Get<Dictionary> for dot-notation support
            var serviceBusConfig = Configuration.GetSection("serviceBus").Get<Dictionary<string, string>>();
            string connectionString = null;
            if (serviceBusConfig != null && serviceBusConfig.TryGetValue("connectionString", out var conn))
            {
                connectionString = conn;
            }

            // Fallback to an app setting named ServiceBusConnectionString if explicit section not present
            connectionString ??= Configuration.GetValue<string>("ServiceBusConnectionString");

            // Create the ServiceBusClient (replaces ProducerConfig)
            var serviceBusClient = new ServiceBusClient(connectionString);
            services.AddSingleton(serviceBusClient);

            // Register default processor options (replaces ConsumerConfig)
            var processorOptions = new ServiceBusProcessorOptions();
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
