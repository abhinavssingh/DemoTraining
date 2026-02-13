using DemoTraining.Models;
using DemoTraining.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Landing.Models;

[SiteContentType(
    DisplayName = "LandingPage",
    GUID = "F0C11C2A-775C-4A70-96A7-477455E457C3",
    Description = "")]
[SiteImageUrl]
public class LandingPage : SitePageData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 310)]
    [CultureSpecific]
    public virtual ContentArea MainContentArea { get; set; }

    //public override void SetDefaultValues(ContentType contentType)
    //{
    //    base.SetDefaultValues(contentType);

    //    HideSiteFooter = true;
    //    HideSiteHeader = true;
    //}
}

