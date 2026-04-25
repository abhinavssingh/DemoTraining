using DemoTraining.Models.ViewModels;

namespace DemoTraining.Features.Search.Models
{
    /// <summary>
    /// STAGE 2 CMS13: Search content model for Graph-based search results
    /// Maps Optimizely Graph responses to view rendering
    /// </summary>
    public class SearchContentModel : PageViewModel<SearchPage>
    {
        public SearchContentModel(SearchPage currentPage)
            : base(currentPage)
        {
        }

        /// <summary>
        /// Indicates if Graph API is operational
        /// Set to true if Graph service is unavailable or misconfigured
        /// </summary>
        public bool SearchServiceDisabled { get; set; }

        /// <summary>
        /// Total number of results matching the query (for pagination UI)
        /// </summary>
        public int NumberOfHits { get; set; }

        /// <summary>
        /// Query parameters from the search request
        /// </summary>
        public SearchQuery SearchedQuery { get; set; } = new SearchQuery();

        /// <summary>
        /// TODO CMS13: Raw Graph facet data (mapping TBD per Graph SDK)
        /// Currently using object placeholder until Graph facet model is finalized
        /// </summary>
        public IEnumerable<object> RawFacets { get; set; }

        /// <summary>
        /// Processed facet buckets for display (sections, content types, etc.)
        /// Defined in FacetBucket.cs
        /// </summary>
        public List<FacetBucket> Facets { get; set; } = new List<FacetBucket>();

        /// <summary>
        /// Search results from Graph query
        /// Each item represents a matched content piece with title, URL, excerpt
        /// </summary>
        public List<object> Results { get; set; }

        /// <summary>
        /// Calculate total pages for pagination
        /// </summary>
        public int TotalPages => SearchedQuery.PageSize > 0
                ? (int)Math.Ceiling(NumberOfHits / (double)SearchedQuery.PageSize) 
                : 0;

        /// <summary>
        /// Error message from search service (if any)
        /// </summary>
        public string SearchErrorMessage { get; set; }
    }
}


