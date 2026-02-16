using DemoTraining.Controllers;
using DemoTraining.Features.Search.Models;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.Search.Controllers;

[TemplateDescriptor(Inherited = true)]
public class SearchPageController : PageControllerBase<SearchPage>
{

    private readonly IClient _client;
    public SearchPageController(IClient client) => _client = client;

    public ViewResult Index(SearchPage currentPage, SearchQuery q)
    {

        q ??= new SearchQuery();

        var query = _client
            .UnifiedSearchFor(q.Q ?? string.Empty)
            .TermsFacetFor(x => x.SearchSection)
            .TermsFacetFor(x => x.SearchCategories)
            .Skip((q.Page - 1) * q.PageSize).Take(q.PageSize);

        if (q.Sections?.Count > 0)
        {
            query = query.FilterHits(h => h.SearchSection.In(q.Sections.ToArray()));
        }

        if (q.Types?.Count > 0)
        {
            query = query.FilterHits(f => f.SearchCategories.In(q.Types.ToArray()));
        }

        var results = query.GetResultAsync().Result;

        var vm = new SearchContentModel(currentPage)
        {
            SearchedQuery = q,
            Results = results,
            RawFacets = results.Facets.OfType<TermsFacet>(),
            SearchServiceDisabled = false,
            NumberOfHits = results.Hits.Count()
        };

        vm.Facets.Add(BuildFacet("Sections", results.TermsFacetFor(x => x.SearchSection), q.Sections));
        vm.Facets.Add(BuildFacet("Content types", results.TermsFacetFor(x => x.SearchCategories), q.Types));

        return View("~/Features/Search/Views/Index.cshtml", vm);
    }


    private FacetBucket BuildFacet(string name, TermsFacet facet,
            List<string> selected)
    {
        var bucket = new FacetBucket { Name = name };
        if (facet?.Terms == null) return bucket;

        foreach (var t in facet.Terms)
        {
            // If the facet term is "for this site", display it as "Media" instead
            var displayTerm = string.Equals(t.Term, "for this site", StringComparison.OrdinalIgnoreCase)
                ? "Media"
                : t.Term;

            bucket.Items.Add(new FacetItem
            {
                // Keep the original term as the checkbox value so filtering still works
                Term = t.Term,
                Display = displayTerm,
                Count = t.Count,
                // Consider the item selected if the original term or the mapped display term is present
                Selected = selected?.Any(s => string.Equals(s, t.Term, StringComparison.OrdinalIgnoreCase)
                                              || string.Equals(s, displayTerm, StringComparison.OrdinalIgnoreCase)) ?? false
            });
        }
        return bucket;
    }
}

