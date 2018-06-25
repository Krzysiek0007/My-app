using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LogViewer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                FileStream fileStream = new FileStream("..\\ActivePlugins\\bin\\Debug\\netcoreapp2.0\\ActivePlugins.log", FileMode.Open);
                StringBuilder log = new StringBuilder();
                log.Append("<b>ActivePlugins Log Viewer</b>");
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        log.Append("<p>" + reader.ReadLine() + "</p>");
                    }
                }
                await context.Response.WriteAsync(log.ToString());
            });
        }
    }
}
