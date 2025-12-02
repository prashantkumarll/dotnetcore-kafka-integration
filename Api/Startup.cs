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
            var producerConfigDict = Configuration.GetSection("producer").Get<Dictionary<string, string>>();
            var consumerConfigDict = Configuration.GetSection("consumer").Get<Dictionary<string, string>>();

            // Create ServiceBusClient using connection string from producer config (key: connectionString)
            var serviceBusConnectionString = producerConfigDict != null && producerConfigDict.TryGetValue("connectionString", out var conn) ? conn : throw new System.InvalidOperationException("Service Bus connection string not configured under 'producer:connectionString'.");
            var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);

            // Create processor options from consumer config if present
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null)
            {
                if (consumerConfigDict.TryGetValue("maxConcurrentCalls", out var maxCalls) && int.TryParse(maxCalls, out var mc))
                {
                    processorOptions.MaxConcurrentCalls = mc;
                }
                if (consumerConfigDict.TryGetValue("autoComplete", out var ac) && bool.TryParse(ac, out var auto))
                {
                    processorOptions.AutoCompleteMessages = auto;
                }
            }

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
