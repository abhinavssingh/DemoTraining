using DemoTraining.Business;
using DemoTraining.Business.Channels;
using DemoTraining.Business.Rendering;
using DemoTraining.Controllers;
using EPiServer.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;


namespace DemoTraining.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDemoTraining(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationExpanders.Add(new SiteViewEngineLocationExpander()));

            services.Configure<DisplayOptions>(displayOption =>
            {
                displayOption.Add("full", "/displayoptions/full", Globals.ContentAreaTags.FullWidth, string.Empty, "epi-icon__layout--full");
                displayOption.Add("wide", "/displayoptions/wide", Globals.ContentAreaTags.WideWidth, string.Empty, "epi-icon__layout--wide");
                displayOption.Add("half", "/displayoptions/half", Globals.ContentAreaTags.HalfWidth, string.Empty, "epi-icon__layout--half");
                displayOption.Add("narrow", "/displayoptions/narrow", Globals.ContentAreaTags.NarrowWidth, string.Empty, "epi-icon__layout--narrow");
            });

            services.Configure<MvcOptions>(options => options.Filters.Add<PageContextActionFilter>());

            services.AddDisplayResolutions();
            services.AddDetection();

            return services;
        }

        private static void AddDisplayResolutions(this IServiceCollection services)
        {
            services.AddSingleton<DisplayResolutions>();
            services.AddSingleton<IpadHorizontalResolution>();
            services.AddSingleton<IphoneVerticalResolution>();
            services.AddSingleton<AndroidVerticalResolution>();
        }

        public static IMvcBuilder AddFeatureFolders(this IMvcBuilder services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddMvcOptions(o => o.Conventions.Add(new ControllerFeatureConvention()))
                .AddRazorOptions(o =>
                {
                    o.ViewLocationFormats.Add(@"{feature}\Views\{0}.cshtml");
                    o.ViewLocationFormats.Add(@"{feature}\{0}.cshtml");
                    o.ViewLocationFormats.Add(@"\Features\{0}.cshtml");

                    o.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                });

            return services;
        }
    }
}
