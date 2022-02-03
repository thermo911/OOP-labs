using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Reports.DAL.Context;
using Reports.DAL.Entities;
using Reports.DAL.Repositories;
using Reports.DAL.Repositories.Impl;
using Reports.Services.Services;
using Reports.Services.Services.Impl;
using Reports.Shrd.Mappers;
using Reports.Shrd.Mappers.Impl;

namespace Reports.Server
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
            services.AddControllers();

            services.AddSingleton<IMapper, SimpleMapper>();
            services.AddSingleton<ReportsContext>();

            services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
            services.AddSingleton<IRepository<Ticket>, TicketRepository>();
            services.AddSingleton<IRepository<Comment>, CommentRepository>();
            services.AddSingleton<IRepository<Report>, ReportRepository>();
            
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReportService, ReportService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Reports.Server", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reports.Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}