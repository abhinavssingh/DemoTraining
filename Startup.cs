using DemoTraining.Extensions;
using EPiServer.Cms.TinyMce.Core;
// TODO CMS13: AspNetIdentity support in EPiServer.Cms.UI has been refactored. CMS 13 handles Identity registration.
using EPiServer.Labs.GridView;
using EPiServer.Scheduler;
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

            // TODO CMS13: CMS 13 core and Identity registration moved to Program.cs via ConfigureCmsDefaults()
            // Startup.cs now handles only app-specific service extensions
            services
                // TODO CMS13: AddCmsTagHelpers may have been moved or renamed in CMS 13. 
                // Tag helpers should be registered via the view configuration or automatically discovered.
                // .AddCmsTagHelpers()
                .AddDemoTraining()
                .AddGridView(options => options.IsViewEnabled = true)
                // STAGE 2 CMS13: Register Optimizely Graph for search functionality
                .AddOptimizelyGraph(_configuration)
                // TODO CMS13: AddAdminUserRegistration is not available in CMS 13 - admin registration may be automatic
                // .AddAdminUserRegistration()
                // TODO CMS13: AddEmbeddedLocalization may have been moved or renamed in CMS 13
                // .AddEmbeddedLocalization<Startup>()
                ;

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Bind EPiServer configuration sections to strongly-typed options
            services.Configure<EpiserverOptions>(_configuration.GetSection("EPiServer"));
            services.Configure<MediaImportOptions>(_configuration.GetSection("EPiServer:MediaImport"));

            // Ensure media import paths are absolute so they work cross-platform and in Docker
            services.PostConfigure<MediaImportOptions>(options =>
            {
                if (!string.IsNullOrEmpty(options.ToImportFolder) && !Path.IsPathRooted(options.ToImportFolder))
                {
                    options.ToImportFolder = Path.Combine(_webHostingEnvironment.ContentRootPath, options.ToImportFolder.Replace('/', Path.DirectorySeparatorChar));
                }

                if (!string.IsNullOrEmpty(options.ImportedFolder) && !Path.IsPathRooted(options.ImportedFolder))
                {
                    options.ImportedFolder = Path.Combine(_webHostingEnvironment.ContentRootPath, options.ImportedFolder.Replace('/', Path.DirectorySeparatorChar));
                }
            });

            services.Configure<TinyMceConfiguration>(config =>
            {
                config.RichtextExtension();

                // firstly comment out the above line and then uncomments and customize the configuration for all properties of type XhtmlString like this:
                // uncomments and customize the configuration for a specific property on a specific content type like this:
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
