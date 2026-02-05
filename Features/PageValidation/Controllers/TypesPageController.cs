using DemoTraining.Features.PageValidation.Models;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DemoTraining.Features.PageValidation.Controllers;

[TemplateDescriptor(Inherited = true)]
public class TypesPageController : PageController<TypesPage>
{
    public ActionResult Index(TypesPage currentPage)
    {
        // Implementation of action. You can create your own view model class that you pass to the view or
        // you can pass the page type model directly for simpler templates

        return View("~/Features/PageValidation/Views/Index.cshtml", currentPage);
    }
}
