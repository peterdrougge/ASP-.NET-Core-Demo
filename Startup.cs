using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreRC2Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Create the configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                // add appSettings if available
                .AddJsonFile("appSettings.json", optional:true, reloadOnChange:true)
                // add environment-specific appSettings if available
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional:true, reloadOnChange:true);
                
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<MyAppSettings>(Configuration);
            services.AddMvc();

             // Create the Autofac container builder.
            var builder = new ContainerBuilder();
		
            // Register dependencies, populate the services from the collection, and build the container.
            builder.RegisterType<DemoInterfaceImpl>().As<IDemoInterface>();
            builder.Populate(services);
            var container = builder.Build();
		
            // Return the IServiceProvider resolved from the container.
            return container.Resolve<IServiceProvider>();

            // not used since we swapped DI to use Autofac instead of the built-in service.
            //services.AddTransient<IDemoInterface, DemoInterfaceImpl>();
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory logger, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                logger.AddConsole(LogLevel.Debug);
                logger.AddDebug();
            }
            else
            {
                logger.AddConsole(LogLevel.Warning);
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseDeveloperExceptionPage();
            app.UseRuntimeInfoPage();
            
            var log = logger.CreateLogger("startup");
            log.LogInformation("we're partying in the Configure part of Startup");
            // needs to be added using .NET CLI or tooling
            // > DOTNET USER-SECRETS SET aspnetcorerc2demo andsomevalueofyourchoice
            log.LogInformation($"User Secrets config data is {Configuration["aspnetcorerc2demo"]}");
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseDemoCustomMiddleware();
            app.UseMvcWithDefaultRoute();
        }

        // uncomment to run this when environment is Production
        // public void ConfigureProduction(IApplicationBuilder app, ILoggerFactory logger, IHostingEnvironment env)
        // {
        //     logger.AddConsole(LogLevel.Warning);
        //     app.UseExceptionHandler("/Home/Error");
            
        //     app.UseDefaultFiles();
        //     app.UseStaticFiles();
            
        //     app.UseDemoCustomMiddleware();
        //     app.UseMvcWithDefaultRoute();
        // }
    }
}