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

            // Resolve connection string from producer or consumer section or fallback to root key
            string serviceBusConnection = null;
            if (producerConfigDict != null && producerConfigDict.TryGetValue("connectionString", out var cs1))
            {
                serviceBusConnection = cs1;
            }
            else if (consumerConfigDict != null && consumerConfigDict.TryGetValue("connectionString", out var cs2))
            {
                serviceBusConnection = cs2;
            }
            else
            {
                serviceBusConnection = Configuration.GetValue<string>("ServiceBusConnectionString");
            }

            var serviceBusClient = new ServiceBusClient(serviceBusConnection);

            // Replace ConsumerConfig with ServiceBusProcessorOptions
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null)
            {
                if (consumerConfigDict.TryGetValue("maxConcurrentCalls", out var mcc) && int.TryParse(mcc, out var parsedMcc))
                {
                    processorOptions.MaxConcurrentCalls = parsedMcc;
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
