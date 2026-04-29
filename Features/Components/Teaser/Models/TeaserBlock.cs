using DemoTraining.Models;
using DemoTraining.Models.Blocks;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Features.Components.Teaser.Models;

/// <summary>
/// Used to provide a stylized entry point to a page on the site
/// </summary>
[SiteContentType(
    DisplayName = "TeaserBlock",
    GUID = "FBFB276A-441B-4D81-B647-3419FAC2C69A",
    Description = "")]
[SiteImageUrl] // Use site's default thumbnail
public class TeaserBlock : SiteBlockData
{
    [CultureSpecific]
    [Required(AllowEmptyStrings = false)]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 1)]
    public virtual string Heading { get; set; }

    [CultureSpecific]
    [Required(AllowEmptyStrings = false)]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 2)]
    [UIHint(UIHint.Textarea)]
    public virtual string Text { get; set; }

    [CultureSpecific]
    [Required(AllowEmptyStrings = false)]
    [UIHint(UIHint.Image)]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 3)]
    public virtual ContentReference Image { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 4)]
    public virtual ContentReference Link { get; set; }
}
