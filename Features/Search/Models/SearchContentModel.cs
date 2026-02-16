using DemoTraining.Models.ViewModels;
using EPiServer.Find.UnifiedSearch;

namespace DemoTraining.Features.Search.Models
{
    public class SearchContentModel : PageViewModel<SearchPage>
    {
        public SearchContentModel(SearchPage currentPage, string searchedQuery)
            : base(currentPage)
        {
            SearchedQuery = searchedQuery;
        }

        public bool SearchServiceDisabled { get; set; }

        public string SearchedQuery { get; private set; }

        public int NumberOfHits { get; set; }


        public UnifiedSearchResults Results { get; set; }
    }
}
