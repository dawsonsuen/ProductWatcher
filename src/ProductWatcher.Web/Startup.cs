using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductWatcher.Apis;

namespace ProductWatcher.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            services.AddDataProtection()
                  .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(env.ContentRootPath, "keys")));

            services.AddMvc();

            var builder = new ContainerBuilder();
            var apiTypes = typeof(IScrapeProduct).Assembly
                        .GetTypes()
                        .Where(x => x.Namespace.StartsWith(typeof(IScrapeProduct).Namespace))
                        .ToArray();

            builder.RegisterTypes(apiTypes).SingleInstance();
            builder.Populate(services);


            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();
            app.Use((cont, del) =>
            {
                var dataProtector = cont.RequestServices.GetRequiredService<IDataProtectionProvider>().CreateProtector("Annon-Session Cookie Provider", "annon-session", "v1");
                if (cont.Request.Cookies.TryGetValue("annon-session", out string val))
                {

                }
                else
                {
                    var protectedData = dataProtector.Protect(Guid.NewGuid().ToString());

                    cont.Response.Cookies.Append("annon-session", Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(protectedData)), new CookieOptions()
                    {
                        SameSite = SameSiteMode.Strict,
                        HttpOnly = true
                    });
                }
                return del.Invoke();
            });
            app.UseMvc();
        }
    }
}
