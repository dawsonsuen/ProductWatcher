using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NPoco;

namespace Aws.ServerlessApi
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>()
            //     .AddDefaultTokenProviders();
            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddScoped<ProductWatcher.Apis.IScrapeProduct, ProductWatcher.Apis.Woolworths.Scraper>();
            services.AddScoped<ProductWatcher.Apis.IScrapeProduct, ProductWatcher.Apis.Coles.Scraper>();
            services.AddResponseCaching();
            services.AddResponseCompression();
            //services.AddScoped<NPoco.Database>(x => new NPoco.Database("", DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance, IsolationLevel.Chaos));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.UseResponseCaching();
            app.UseResponseCompression();

        }
    }
}
