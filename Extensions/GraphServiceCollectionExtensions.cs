using DemoTraining.Features.Search.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoTraining.Extensions
{
    /// <summary>
    /// STAGE 2 CMS13: Optimizely Graph SDK configuration
    /// Provides setup for Graph-based search functionality
    /// </summary>
    public static class GraphServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Optimizely Graph client and search service for content queries
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddOptimizelyGraph(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Graph search service for dependency injection
            services.AddScoped<IGraphSearchService, GraphSearchService>();

            // TODO CMS13: Add Graph SDK client registration once packages are available
            // services.AddHttpClient<IGraphQueryClient>()
            //     .ConfigureHttpClient((sp, client) =>
            //     {
            //         var graphConfig = configuration.GetSection("Optimizely:Graph");
            //         client.BaseAddress = new Uri(graphConfig["Gateway"]);
            //         client.DefaultRequestHeaders.Add("X-API-Key", graphConfig["ApiKey"]);
            //     });

            return services;
        }
    }
}

