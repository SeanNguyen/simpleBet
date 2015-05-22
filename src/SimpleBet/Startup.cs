using Microsoft.Framework.DependencyInjection;
using SimpleBet.Models;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Builder;
using Microsoft.Data.Entity;

namespace SimpleBet
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            this.Configuration = new Configuration()
                .AddJsonFile("Config.json")
                .AddEnvironmentVariables();
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //setup database
            var connectionString = this.Configuration.Get("Data:DefaultConnection:ConnectionString");
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<SimpleBetContext>(options =>
                {
                    options.UseSqlServer(this.Configuration.Get("Data:DefaultConnection:ConnectionString"));
                });
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
        }
    }
}
