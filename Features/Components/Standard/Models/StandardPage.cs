using DemoTraining.Models;
using DemoTraining.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Components.Standard.Models;

[SiteContentType(
    DisplayName = "StandardPage",
    GUID = "A833C05D-8E8D-4571-852A-04116C275059",
    Description = "")]
[SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
public class StandardPage : SitePageData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 310)]
    [CultureSpecific]
    public virtual XhtmlString MainBody { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 320)]
    public virtual ContentArea MainContentArea { get; set; }
}
