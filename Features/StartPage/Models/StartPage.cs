using DemoTraining.Features.Components.SitelogoType.Models;
using DemoTraining.Features.Landing.Models;
using DemoTraining.Features.Products.Models;
using DemoTraining.Features.Search.Models;
using DemoTraining.Features.Standard.Models;
using DemoTraining.Models;
using DemoTraining.Models.Pages;
using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.StartPage.Models;

[SiteContentType(
    DisplayName = "StartPage",
    GUID = "A9AEE49E-8995-4635-9D9B-645248D5E8B6",
    Description = "")]
[SiteImageUrl]
[AvailableContentTypes(
    Availability.Specific,
    Include =
    [
        typeof(ContainerPage),
        typeof(ProductPage),
        typeof(StandardPage),
        typeof(ISearchPage),
        typeof(LandingPage),
        typeof(ContentFolder)
    ], // Pages we can create under the start page...
    ExcludeOn =
    [
        typeof(ContainerPage),
        typeof(ProductPage),
        typeof(StandardPage),
        typeof(ISearchPage),
        typeof(LandingPage)
    ])] // ...and underneath those we can't create additional start pages
public class StartPage : SitePageData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 320)]
    [CultureSpecific]
    public virtual ContentArea MainContentArea { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 300)]
    public virtual LinkItemCollection ProductPageLinks { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 350)]
    public virtual LinkItemCollection ResourcePageLinks { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 400)]
    public virtual LinkItemCollection CompanyPageLinks { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings, Order = 450)]
    public virtual LinkItemCollection SupportPageLinks { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings)]
    public virtual PageReference GlobalNewsPageLink { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings)]
    public virtual PageReference ContactsPageLink { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings)]
    public virtual PageReference SearchPageLink { get; set; }

    [Display(GroupName = Globals.GroupNames.SiteSettings)]
    public virtual SiteLogotypeBlock SiteLogotype { get; set; }
}
