using DemoTraining.Features.Components.Jumbotron.Models;
using DemoTraining.Features.Components.Standard.Models;
using DemoTraining.Models;
using DemoTraining.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Products.Models;

[SiteContentType(
    DisplayName = "ProductPage",
    GUID = "B1B6571D-FD98-4E4C-A03E-8411954727A7",
    Description = "", GroupName = Globals.GroupNames.Products)]
[SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-product.png")]
[AvailableContentTypes(
    Availability = Availability.Specific,
    IncludeOn = [typeof(DemoTraining.Features.StartPage.Models.StartPage)])]
public class ProductPage : StandardPage, IHasRelatedContent
{
    [Required]
    [Display(Order = 305)]
    [UIHint(Globals.SiteUIHints.StringsCollection)]
    [CultureSpecific]
    public virtual IList<string> UniqueSellingPoints { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 330)]
    [CultureSpecific]
    [AllowedTypes([typeof(IContentData)], [typeof(JumbotronBlock)])]
    public virtual ContentArea RelatedContentArea { get; set; }
}
