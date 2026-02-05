using DemoTraining.Features.CmsFieldTypes.Models;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.CmsFieldTypes.Controllers;

[TemplateDescriptor(Inherited = true)]
public class PropertyTypesDemoController : PageController<PropertyTypesDemoPage>
{
    public ActionResult Index(PropertyTypesDemoPage currentPage)
    {
        // Implementation of action. You can create your own view model class that you pass to the view or
        // you can pass the page type model directly for simpler templates

        return View("~/Features/CmsFieldTypes/Views/Index.cshtml", currentPage);
    }
}
