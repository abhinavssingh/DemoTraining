using DemoTraining.Models.ViewModels;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.UnifiedSearch;

namespace DemoTraining.Features.Search.Models
{
    public class SearchContentModel : PageViewModel<SearchPage>
    {
        public SearchContentModel(SearchPage currentPage)
            : base(currentPage)
        {
        }

        public bool SearchServiceDisabled { get; set; }

        public int NumberOfHits { get; set; }

        public SearchQuery SearchedQuery { get; set; } = new SearchQuery();
        public IEnumerable<TermsFacet> RawFacets { get; set; }
        public List<FacetBucket> Facets { get; set; } = new List<FacetBucket>();
        public UnifiedSearchResults Results { get; set; }
        public int TotalPages => SearchedQuery.PageSize > 0
                ? (int)System.Math.Ceiling(NumberOfHits / (double)SearchedQuery.PageSize) : 0;
    }
}
