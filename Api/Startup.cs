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

            // Bind configuration sections - keep using Dictionary for dot-notation support if present
            var producerConfigDict = Configuration.GetSection("producer").Get<Dictionary<string, string>>();
            var consumerConfigDict = Configuration.GetSection("consumer").Get<Dictionary<string, string>>();

            // Construct ServiceBusClient using a connection string from configuration.
            // Prefer "connectionString" key in producer section for backward compatibility with previous config layout.
            string connectionString = null;
            if (producerConfigDict != null)
            {
                producerConfigDict.TryGetValue("connectionString", out connectionString);
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Configuration.GetValue<string>("ServiceBus:ConnectionString");
            }

            var serviceBusClient = new ServiceBusClient(connectionString);

            // Map consumer config dictionary to ServiceBusProcessorOptions if needed
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null)
            {
                if (consumerConfigDict.TryGetValue("autoCommit", out var autoCommit))
                {
                    // autoCommit false -> AutoCompleteMessages = false
                    processorOptions.AutoCompleteMessages = autoCommit.Equals("true", System.StringComparison.OrdinalIgnoreCase);
                }
                if (consumerConfigDict.TryGetValue("max.poll.interval.ms", out var maxConcurrent))
                {
                    if (int.TryParse(maxConcurrent, out var max))
                    {
                        processorOptions.MaxConcurrentCalls = max;
                    }
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
