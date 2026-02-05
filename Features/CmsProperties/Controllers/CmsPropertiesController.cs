using DemoTraining.Features.CmsProperties.Models;
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

        return View("~/Features/CmsProperties/Views/Index.cshtml", currentPage);
    }
}
