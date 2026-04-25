using EPiServer.Core;

namespace DemoTraining.Features.Search.Services
{
    /// <summary>
    /// PHASE 4 CMS13: Abstraction layer for Graph-based search
    /// Provides async search interface compatible with SearchPageController
    /// </summary>
    public interface IGraphSearchService
    {
        /// <summary>
        /// Execute a content search using Optimizely Graph
        /// </summary>
        /// <param name="query">Search query text</param>
        /// <param name="filters">Optional content type/section filters</param>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Results per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Search results with items and metadata</returns>
        Task<GraphSearchResult> SearchAsync(
            string query, 
            GraphSearchFilters filters = null, 
            int page = 1, 
            int pageSize = 20,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Search filters for Graph queries
    /// </summary>
    public class GraphSearchFilters
    {
        /// <summary>Content sections/categories to filter by</summary>
        public List<string> Sections { get; set; } = new();

        /// <summary>Content types to filter by (page types, block types, etc.)</summary>
        public List<string> ContentTypes { get; set; } = new();

        /// <summary>Only show published content</summary>
        public bool OnlyPublished { get; set; } = true;

        /// <summary>Visitor mode (respect access control)</summary>
        public bool ApplyAccessControl { get; set; } = true;
    }

    /// <summary>
    /// Individual search result item from Graph
    /// </summary>
    public class GraphSearchResultItem
    {
        /// <summary>Content ID</summary>
        public ContentReference ContentLink { get; set; }

        /// <summary>Content display name/title</summary>
        public string Title { get; set; }

        /// <summary>Brief excerpt or description</summary>
        public string Excerpt { get; set; }

        /// <summary>Direct URL to content</summary>
        public string Url { get; set; }

        /// <summary>Content type name</summary>
        public string ContentType { get; set; }

        /// <summary>Publishing date</summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>Content hierarchy section/category</summary>
        public string Section { get; set; }

        /// <summary>Relevance score from Graph (0-1)</summary>
        public float RelevanceScore { get; set; }
    }

    /// <summary>
    /// Graph search result collection
    /// Maps Graph API response to DemoTraining search model
    /// </summary>
    public class GraphSearchResult
    {
        /// <summary>Search results matching query</summary>
        public List<GraphSearchResultItem> Items { get; set; } = new();

        /// <summary>Total number of matching results (for pagination UI)</summary>
        public int TotalCount { get; set; }

        /// <summary>Current page number</summary>
        public int Page { get; set; } = 1;

        /// <summary>Results per page</summary>
        public int PageSize { get; set; } = 20;

        /// <summary>Total pages available</summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        /// <summary>Available facets/aggregations (e.g., sections, types)</summary>
        public Dictionary<string, List<FacetCount>> Facets { get; set; } = new();

        /// <summary>Indicates if Graph API is available and responding</summary>
        public bool IsGraphServiceAvailable { get; set; } = true;

        /// <summary>Error message if search failed</summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Facet count for aggregations
    /// </summary>
    public class FacetCount
    {
        /// <summary>Facet value (e.g., section name, content type)</summary>
        public string Value { get; set; }

        /// <summary>Number of items with this facet value</summary>
        public int Count { get; set; }
    }
}
