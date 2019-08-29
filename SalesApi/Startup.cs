using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
<<<<<<< HEAD
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
=======
>>>>>>> 7787b64ef79fb821fa293c96d5d5f33295c2e7ac

namespace SalesApi
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
<<<<<<< HEAD
         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ApiForSales", Version = "v1" });
            });
        }
=======
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
>>>>>>> 7787b64ef79fb821fa293c96d5d5f33295c2e7ac

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

<<<<<<< HEAD
            app.UseSwagger();

            // Ativa o Swagger UI
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProject V1");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProject V1");
                c.RoutePrefix = string.Empty;
            });
=======
>>>>>>> 7787b64ef79fb821fa293c96d5d5f33295c2e7ac

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
