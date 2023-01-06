using RedisApp.Classes;
using RedisApp.Classes.Settings;

namespace RedisApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // get variables appseetting json

            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            // injection dependency

            IRedisSettings redisSettings=new RedisSettings();
            Configuration.Bind("RedisSettings", redisSettings);//  Asign values to redisSettings
            services.AddSingleton<IRedisSettings>(redisSettings);// injection dependency
            
            services.AddTransient<IRedisAdapter,RedisAdapter>();// injection dependency AdapterRedis

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
