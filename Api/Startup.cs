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

            // Create ServiceBusClient from configuration (replaces ProducerConfig)
            var serviceBusConnection = Configuration.GetValue<string>("ServiceBus:ConnectionString");
            var serviceBusClient = new ServiceBusClient(serviceBusConnection);

            // Create processor options (replaces ConsumerConfig)
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null && consumerConfigDict.TryGetValue("maxConcurrentCalls", out var mcc) && int.TryParse(mcc, out var max))
            {
                processorOptions.MaxConcurrentCalls = max;
            }
            if (consumerConfigDict != null && consumerConfigDict.TryGetValue("autoCompleteMessages", out var ac) && bool.TryParse(ac, out var autoComplete))
            {
                processorOptions.AutoCompleteMessages = autoComplete;
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
