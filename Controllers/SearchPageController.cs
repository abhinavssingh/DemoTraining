using DemoTraining.Features.Search.Models;
using EPiServer.Find;
using EPiServer.Find.Framework;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Controllers;

[TemplateDescriptor(Inherited = true)]
public class SearchPageController : PageControllerBase<SearchPage>
{
    public ViewResult Index(SearchPage currentPage, string q)
    {
        var model = new SearchContentModel(currentPage, q);
        if (String.IsNullOrEmpty(q))
        {
            return View(model);
        }

        var unifiedSearch = SearchClient.Instance.UnifiedSearchFor(q);
        var results = unifiedSearch.GetResultAsync().Result;
        var resultModel = new SearchContentModel(currentPage, q)
        {
            Results = results,
            NumberOfHits = results.Hits.Count(),
            SearchServiceDisabled = false,

        };

        return View("~/Features/Search/Views/Index.cshtml", resultModel);
    }
}

