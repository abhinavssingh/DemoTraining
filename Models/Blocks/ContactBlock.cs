using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoTraining.Models.Blocks;

/// <summary>
/// Used to present contact information with a call-to-action link
/// </summary>
/// <remarks>Actual contact details are retrieved from a contact page specified using the ContactPageLink property</remarks>
[SiteContentType(
    DisplayName = "ContactBlock",
    GUID = "A7822779-2AF8-4E57-BCDA-F41FD2C8C3E7",
    Description = "")]
[SiteImageUrl]
public class ContactBlock : SiteBlockData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 1)]
    [CultureSpecific]
    [UIHint(UIHint.Image)]
    public virtual ContentReference Image { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 2)]
    [CultureSpecific]
    public virtual string Heading { get; set; }

    /// <summary>
    /// Gets or sets the contact page from which contact information should be retrieved
    /// </summary>
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 3)]
    [UIHint(Globals.SiteUIHints.Contact)]
    public virtual PageReference ContactPageLink { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 4)]
    [CultureSpecific]
    public virtual string LinkText { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 5)]
    [CultureSpecific]
    public virtual Url LinkUrl { get; set; }
}

