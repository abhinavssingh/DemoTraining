using DemoTraining.Controllers;
using DemoTraining.Features.Search.Models;
using DemoTraining.Features.Search.Services;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Search.Controllers;

/// <summary>
/// STAGE 2 CMS13: Search controller migrated from EPiServer.Find to Optimizely Graph
/// </summary>
[TemplateDescriptor(Inherited = true)]
public class SearchPageController : PageControllerBase<SearchPage>
{
    private readonly IGraphSearchService _searchService;
    private readonly ILogger<SearchPageController> _logger;

    /// <summary>
    /// Initialize search controller with Graph search service
    /// </summary>
    /// <param name="searchService">Optimizely Graph search service (async-first)</param>
    /// <param name="logger">Logging</param>
    public SearchPageController(IGraphSearchService searchService, ILogger<SearchPageController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    /// <summary>
    /// Async search action using Optimizely Graph
    /// Replaces legacy IClient.UnifiedSearchFor() pattern
    /// </summary>
#pragma warning disable MVC1004 // Model binding ambiguity resolved via explicit [FromQuery] attribute
    public async Task<ViewResult> Index(SearchPage currentPage, [FromQuery(Name = "q")] SearchQuery query, CancellationToken cancellationToken = default)
#pragma warning restore MVC1004
    {
        query ??= new SearchQuery();

        // Build filter criteria from query parameters
        var filters = new GraphSearchFilters
        {
            Sections = query.Sections ?? new List<string>(),
            ContentTypes = query.Types ?? new List<string>(),
            OnlyPublished = true,
            ApplyAccessControl = true
        };

        // TODO CMS13: Once Graph SDK is available, this async call will use actual Graph API
        // Placeholder implementation returns empty results with message
        GraphSearchResult searchResult = await _searchService.SearchAsync(
            query.Q ?? string.Empty,
            filters,
            query.Page,
            query.PageSize,
            cancellationToken);

        // Map Graph results to content model for view rendering
        var vm = new SearchContentModel(currentPage)
        {
            SearchedQuery = query,
            Results = MapSearchResults(searchResult),
            RawFacets = MapFacets(searchResult),
            SearchServiceDisabled = !searchResult.IsGraphServiceAvailable,
            NumberOfHits = searchResult.TotalCount
        };

        // Add facets to model for UI rendering
        if (searchResult.Facets?.Count > 0)
        {
            vm.Facets.Add(BuildFacet("Sections", searchResult.Facets, query.Sections));
            vm.Facets.Add(BuildFacet("Content types", searchResult.Facets, query.Types));
        }

        _logger.LogInformation($"Search completed: {query.Q} returned {searchResult.TotalCount} results");

        return View("~/Features/Search/Views/Index.cshtml", vm);
    }

    /// <summary>
    /// Map Graph search results to view-compatible format
    /// </summary>
    private List<object> MapSearchResults(GraphSearchResult graphResults)
    {
        if (!graphResults.IsGraphServiceAvailable || graphResults.Items == null)
            return new List<object>();

        return graphResults.Items
            .Select(item => new
            {
                item.Title,
                item.Url,
                item.Excerpt,
                item.ContentType
            })
            .Cast<object>()
            .ToList();
    }

    /// <summary>
    /// Map Graph facets to view-compatible format
    /// TODO CMS13: Update when Graph facet model is finalized
    /// </summary>
    private IEnumerable<object> MapFacets(GraphSearchResult graphResults)
    {
        if (graphResults.Facets == null || graphResults.Facets.Count == 0)
            return Enumerable.Empty<object>();

        // TODO: Map Graph facet structure to view expectations
        return graphResults.Facets.Values
            .SelectMany(f => f)
            .Cast<object>();
    }

    /// <summary>
    /// Build facet bucket for UI display
    /// </summary>
    private FacetBucket BuildFacet(
        string name,
        Dictionary<string, List<FacetCount>> facets,
        List<string> selected)
    {
        var bucket = new FacetBucket { Name = name };

        // TODO CMS13: Map Graph facets to FacetBucket structure
        // This is a placeholder pending final Graph facet API design

        return bucket;
    }
}


