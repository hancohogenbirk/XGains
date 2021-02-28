using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using XGains.BackgroundServices;
using XGains.Clients;
using XGains.Data;
using XGains.Hubs;
using XGains.Options;
using XGains.Providers;

namespace XGains
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;


            if (_environment.IsDevelopment())
            {
                Configuration = new ConfigurationBuilder()
                    .AddConfiguration(configuration)
                    .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json")
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Startup>()
                    .Build();
                return;
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:44367")
                });

            services
                .AddRazorPages()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddHostedService<ClockBackgroundService>();
            services.AddHostedService<KrakenBackgroundService>();
            services.AddSignalR();

            services.Configure<KrakenClientOptions>(Configuration.GetSection(nameof(KrakenClientOptions)));

            services.AddScoped<IKrakenClient, KrakenClient>();

            services.AddScoped<ISignatureProvider, SignatureProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHub<ClockHub>("/clockhub");
                endpoints.MapHub<BalanceHub>("/balancehub");
            });
        }
    }
}
