using Serilog;
using Serilog.Core;
using ChatApplication.BusinessLogic;
using Microsoft.Extensions.DependencyModel;

namespace QueueAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private Logger log;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            log = new LoggerConfiguration()
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
              .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
              .Enrich.FromLogContext()
              .WriteTo.File(@"Logs\" + Date + "\\log-.txt", rollingInterval: RollingInterval.Hour)
              .CreateLogger();
            Log.Information("Application Starting");

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Serilog.ILogger>(log);
            services.AddSingleton(new ChatManager());


            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSerilogRequestLogging();

        }
    }
}
