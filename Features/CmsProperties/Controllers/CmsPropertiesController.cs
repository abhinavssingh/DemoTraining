using DemoTraining.Features.CmsProperties.Models;
using DemoTraining.Models.Pages;
using DemoTraining.Models.ViewModels;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.CmsProperties.Controllers;

[TemplateDescriptor(Inherited = true)]
public class CmsPropertiesController : PageController<CmsPropertiesPage>
{
    public ActionResult Index(CmsPropertiesPage currentPage)
    {
        // Implementation of action. You can create your own view model class that you pass to the view or
        // you can pass the page type model directly for simpler templates
        var model = CreateModel(currentPage);
        return View("~/Features/CmsProperties/Views/Index.cshtml", model);
    }

    /// <summary>
    /// Creates a PageViewModel where the type parameter is the type of the page.
    /// </summary>
    /// <remarks>
    /// Used to create models of a specific type without the calling method having to know that type.
    /// </remarks>
    private static IPageViewModel<SitePageData> CreateModel(SitePageData page)
    {
        var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
        return Activator.CreateInstance(type, page) as IPageViewModel<SitePageData>;
    }
}
