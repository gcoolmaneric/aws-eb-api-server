namespace API_SERVER
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;

    public class Startup
    {
        public ILoggerFactory loggerFactory;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this._logger = logger;
        }
      
        public IConfiguration Configuration { get; }
        private ILogger<Startup> _logger;

        public void Configure(IApplicationBuilder app)
        {
            app.Map("/checkStatus", config => config.Run(async context =>
            {
                // Log request parameters
                string queryStr = context.Request.QueryString.ToString();
                _logger.LogInformation("request:" + queryStr);

                try
                {
                    // Log Hello World"
                    _logger.LogInformation("Hello World");

                    await context.Response.WriteAsync("Hello World");

                }
                catch (Exception exception)
                {

                    context.Response.StatusCode = 500;

                    // Log exception error
                    _logger.LogError("exception errorMsg:" + exception.Message);
                    await context.Response.WriteAsync("Internal Server Error"); 
                }
            }));


            app.Run(async context =>
            {
                // Log status
                _logger.LogInformation("API Server is alive");

                await context.Response.WriteAsync("API Server is alive.");
            });
        }

    }
}
