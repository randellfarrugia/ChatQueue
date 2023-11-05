using Serilog;
using Serilog.Core;
using ChatApplication.BusinessLogic;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace QueueAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private Logger log;
        private readonly IHttpContextAccessor _httpContextAccessor;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Serilog.ILogger>(log);
            services.AddSingleton<IChatManager, ChatManager>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
