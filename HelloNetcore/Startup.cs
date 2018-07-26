using System.Linq;
using System.Reflection;
using HelloNetcore.Data;
using HelloNetcore.Models;
using HelloNetcore.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace HelloNetcore
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

            const string connectString = "Server=LUKE\\SQLEXPRESS;Database=IdentityServer4;Trusted_Connection=True;MultipleActiveResultSets=true";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        {
                            builder.UseSqlServer(connectString, sql => sql.MigrationsAssembly(migrationsAssembly));
                        };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(connectString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    };
                })

                //.AddInMemoryClients(Config.GetClients())
                ////.AddTestUsers(Config.GetTestUser())
                //.AddInMemoryApiResources(Config.GetResouces())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddAspNetIdentity<ApplicationUser>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ConsentService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            InitIdentityServerDataBase(app);
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        public void InitIdentityServerDataBase(IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                }

                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var api in Config.GetResouces())
                    {
                        configurationDbContext.ApiResources.Add(api.ToEntity());
                    }
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var identity in Config.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(identity.ToEntity());
                    }
                }

                configurationDbContext.SaveChanges();
            }


        }
    }
}
