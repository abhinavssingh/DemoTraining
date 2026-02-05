using DemoTraining.Features.Search.Models;
using DemoTraining.Models.ViewModels;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Controllers;

[TemplateDescriptor(Inherited = true)]
public class SearchPageController : PageControllerBase<SearchPage>
{
    public ViewResult Index(SearchPage currentPage, string q)
    {
        var model = new SearchContentModel(currentPage)
        {
            Hits = Enumerable.Empty<SearchContentModel.SearchHit>(),
            NumberOfHits = 0,
            SearchServiceDisabled = true,
            SearchedQuery = q
        };

        return View("~/Features/Search/Views/Index.cshtml", model);
    }
}

