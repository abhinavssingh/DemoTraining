using DemoTraining.Controllers;
using DemoTraining.Models.ViewModels;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Page = DemoTraining.Features.StartPage.Models;

namespace DemoTraining.Features.StartPage.Controllers;

[TemplateDescriptor(Inherited = true)]
public class StartPageController : PageControllerBase<Page.StartPage>
{
    public IActionResult Index(Page.StartPage currentPage)
    {
        var model = PageViewModel.Create(currentPage);

        // Check if it is the StartPage or just a page of the StartPage type.
        // TODO CMS13: SiteDefinition.Current is deprecated. Use IApplicationResolver instead.
        if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink))
        {
            // Connect the view models logotype property to the start page's to make it editable
            var editHints = ViewData.GetEditHints<PageViewModel<Page.StartPage>, Page.StartPage>();
            editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
            editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
            editHints.AddConnection(m => m.Layout.ResourcePages, p => p.ResourcePageLinks);
            editHints.AddConnection(m => m.Layout.CompanyPages, p => p.CompanyPageLinks);
            editHints.AddConnection(m => m.Layout.SupportPages, p => p.SupportPageLinks);
        }

        return View("~/Features/StartPage/Views/Index.cshtml", model);
    }
}