using System;
using System.Net;
using congestion_tax_calculator.Model.Interfaces;
using congestion_tax_calculator_net_core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace congestion_tax_calculator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "congestion_tax_calculator.Api", Version = "v1"});
            });
            
            // Dependencies
            services.AddScoped<ICongestionTaxCalculator, CongestionTaxCalculator>();
            services.AddScoped<ICongestionTaxPriceGetter, CongestionTaxPriceGetter>();
            services.AddScoped<ITollFreeDateChecker, TollFreeDateChecker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "congestion_tax_calculator.Api v1"));
            }

            EnableGlobalExceptionHandling(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private static void EnableGlobalExceptionHandling(IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                string result;

                if (exception is ArgumentException argumentException)
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        error = exception.Message
                    });
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest; 
                }
                else
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        error = "Ooops, it's not you, it's us!"
                    });
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
                
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
        }
    }
}