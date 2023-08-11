using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace CoreVsApp
{
    public class Startup
    {
        IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            ////���������� ����� Use, ����� ������ ����������� ������ �� ���������
            //app.Use(async (context, next) =>
            //{
            //    // ������ ��� ���������� � ���
            //    string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            //    // ���� �� ���� (�����-����, ���������� �������� IWebHostEnvironment)
            //    string logFilePath = Path.Combine(env.ContentRootPath, "Logs", "RequestLog.txt");

            //    // ���������� ����������� ������ � ����
            //    await File.AppendAllTextAsync(logFilePath, logMessage);

            //    await next.Invoke();
            //});

            // ���������� ����������� � �������������� �� �������������� ����
            app.UseMiddleware<LoggingMiddleware>();

            //��������� ��������� � ���������� ��������� ��� ������� ��������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            // ��� ������ �������� ����� ��������� �����������
            app.Map("/about", About);
            app.Map("/config", Config);

            // ���������� ��� ������ "�������� �� �������"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });
        }

        /// <summary>
        ///  ���������� ��� �������� About
        /// </summary>
        private void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{_env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  ���������� ��� ������� ��������
        /// </summary>
        private void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {_env.ApplicationName}. App running configuration: {_env.EnvironmentName}");
            });
        }
    }
}
