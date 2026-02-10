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

        public Startup(IWebHostEnvironment webHostingEnvironment)
        {
            _webHostingEnvironment = webHostingEnvironment;
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


            app.UseStaticFiles();
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
