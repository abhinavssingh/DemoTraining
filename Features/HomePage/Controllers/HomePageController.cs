using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Home = DemoTraining.Features.HomePage.Models;

namespace DemoTraining.Features.HomePage.Controllers;

[TemplateDescriptor(Inherited = true)]
public class HomePageController : PageController<Home.HomePage>
{
    public ActionResult Index(Home.HomePage currentPage)
    {
        // Implementation of action. You can create your own view model class that you pass to the view or
        // you can pass the page type model directly for simpler templates

        return View("~/Features/HomePage/Views/Index.cshtml", currentPage);
    }
}
