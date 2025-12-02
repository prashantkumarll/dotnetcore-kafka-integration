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

            // Create ServiceBusClient from producer config (expects "connectionString" key)
            producerConfigDict = producerConfigDict ?? new Dictionary<string, string>();
            producerConfigDict.TryGetValue("connectionString", out var connectionString);
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback to a common configuration path if specific key is not present
                connectionString = Configuration.GetValue<string>("ServiceBus:ConnectionString");
            }

            var client = new ServiceBusClient(connectionString);

            // Build processor options from consumer config (if present)
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null)
            {
                if (consumerConfigDict.TryGetValue("MaxConcurrentCalls", out var maxCallsStr) && int.TryParse(maxCallsStr, out var maxCalls))
                {
                    processorOptions.MaxConcurrentCalls = maxCalls;
                }
                if (consumerConfigDict.TryGetValue("AutoCompleteMessages", out var autoCompleteStr) && bool.TryParse(autoCompleteStr, out var autoComplete))
                {
                    processorOptions.AutoCompleteMessages = autoComplete;
                }
            }

            services.AddSingleton(client);
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
