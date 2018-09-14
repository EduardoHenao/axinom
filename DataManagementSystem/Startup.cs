using AxinomCommon.IServices;
using AxinomCommon.Services;
using DataManagementSystem.IServices;
using DataManagementSystem.Repositories;
using DataManagementSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataManagementSystem
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MVC
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // EF
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<DataManagementSystemContext>(
                options => options.UseSqlServer(_configuration["ConnectionString"], providerOptions => providerOptions.CommandTimeout(60)));

            // services injection
            services.AddTransient<IFileManagementServices, FileManagementServices>();
            services.AddTransient<IZipServices, ZipServices>();
            services.AddTransient<IEncryptionServices, EncryptionServices>();
            services.AddTransient<IPersistenceServices, PersistenceServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
