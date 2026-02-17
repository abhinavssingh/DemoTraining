using DemoTraining.Extensions;
using EPiServer.Cms.Shell;
using EPiServer.Cms.TinyMce.Core;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Labs.GridView;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace DemoTraining
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_webHostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<SchedulerOptions>(options => options.Enabled = false);
            }

            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddFind()
                .AddCmsTagHelpers()
                .AddDemoTraining()
                .AddGridView(options => options.IsViewEnabled = true)
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Startup>();


            // Required by Wangkanai.Detection
            services.AddDetection();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddContentDeliveryApi(options =>
            {
                options.SiteDefinitionApiEnabled = true;
            });

            // Bind EPiServer configuration sections to strongly-typed options
            services.Configure<EpiserverOptions>(_configuration.GetSection("EPiServer"));
            services.Configure<MediaImportOptions>(_configuration.GetSection("EPiServer:MediaImport"));

            services.Configure<TinyMceConfiguration>(config =>
            {
                config.RichtextExtension();

                // firstlu comment out the above line and then uncooments and customize the configuration for all properties of type XhtmlString like this:
                // uncooments and customize the configuration for a specific property on a specific content type like this:
                //var customConfigForStandardPage = config
                //.Default() // Start with default settings or another named configuration
                //.AddPlugin("table") // Add specific plugins (e.g., source code view)
                //.AppendToolbar("table epi-link | bold italic"); // Customize the toolbar button

                //config.For<StandardPage>(x => x.MainBody, customConfigForStandardPage);
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Required by Wangkanai.Detection
            app.UseDetection();
            app.UseSession();


            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Disable caching for static files in development
                    ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                    ctx.Context.Response.Headers.Append("Pragma", "no-cache");
                    ctx.Context.Response.Headers.Append("Expires", "0");
                }
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();
            });
        }
    }
}
