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

            // Bind Service Bus configuration and register client/options
            var serviceBusConnectionString = Configuration.GetConnectionString("ServiceBus") ?? Configuration["ServiceBus:ConnectionString"];

            services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(serviceBusConnectionString));

            var consumerConfigDict = Configuration.GetSection("consumer").Get<Dictionary<string, string>>();
            var processorOptions = new ServiceBusProcessorOptions();
            if (consumerConfigDict != null)
            {
                if (consumerConfigDict.TryGetValue("MaxConcurrentCalls", out var max) && int.TryParse(max, out var maxVal))
                {
                    processorOptions.MaxConcurrentCalls = maxVal;
                }

                if (consumerConfigDict.TryGetValue("AutoCompleteMessages", out var auto) && bool.TryParse(auto, out var autoVal))
                {
                    processorOptions.AutoCompleteMessages = autoVal;
                }
            }

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
